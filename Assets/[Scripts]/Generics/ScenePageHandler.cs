using Mediapipe.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Will handle the pages and splash screen pop up etc.
// Basically all the common things in a scene
public class ScenePageHandler : MonoBehaviour
{
    // temp list
    public List<PageLogic> pageLogics;
    private PageLogic currentPageLogic;
    int currentPageIndex = 0;
    [Header("Managers")]
    public LandmarkManager landmarkManager;
    public HandGestureManager handGestureManager;


    //[Header("Splash")]
    //public GameObject splashArt;
    //public Button bu_closeSplashArt;
    [Header("Splash Page")]
    public SplashScreen splashScreen;
    public UISpriteAnimation sceneAnimationStart;
    public UISpriteAnimation sceneAnimationEnd;
    public Transform headertransform;
    public Image img_splashIcon;
    public TMP_Text tmp_description;

    [Header("Detection Icon")]
    public Image img_face;
    public Image img_hand;
    [Header("Pages")]
    private bool gestureNavigationDetected = false;
    public Button bu_prevPage;
    public Button bu_nextPage;
    public GestureSO gestureNavigation;
    public Image img_nextPageFilled;
    public float nextPageTime = 3f;
    private float nextPageTimeElapsed;

    [Header("Annotation screens")]
    public GameObject annotationFace;
    public GameObject annotationHand;
    //[Header("Annotation List of Points")]
    //public LandmarkListGO annotationList_FacePoints;
    //public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints;
    //List<GameObject> facePoints = new List<GameObject>();
    //List<HandPointAnnotationObject> handPoints = new List<HandPointAnnotationObject>();


    bool handPointsStored = false;
    private void Start()
    {
        bu_nextPage.onClick.AddListener(() => { PreNextPage(); });
        // go to next page after scene animation
        sceneAnimationEnd.OnAnimationFinished += NextPage;


        landmarkManager.SubscribePointsActive(OnFaceSpotted, OnHandSpotted);
        for (int i = 0; i < pageLogics.Count; i++)
        {
            PageLogic_Extension pageLogic_Extension = pageLogics[i] as PageLogic_Extension;
            if(pageLogic_Extension!= null)
            {
                landmarkManager.SubscribePointsActive(pageLogic_Extension.OnFaceAnnotationActive, pageLogic_Extension.OnHandAnnotationActive);
            }
        }

        // subscribe gesture events
        handGestureManager.SubscribeFirstDetected(OnGestureFirstDetected);
        handGestureManager.SubscribeDetecting(OnGestureDetecting);


        // Disable all pages
        for (int i = 0; i < pageLogics.Count; i++)
            pageLogics[i].gameObject.SetActive(false);


        StartPage();
    }

    private void OnFaceSpotted(bool active)
    {
            img_face.gameObject.SetActive(active);
    }

    private void OnHandSpotted(bool active)
    {
        img_hand.gameObject.SetActive(active);
    }

    private void Update()
    {
        if (!sceneAnimationEnd.IsDone()) return;
        // find annotation points

        if (currentPageLogic != null)
        {
            // run curring page logic
            if (currentPageLogic.CompareState(PageLogic.PageState.START))
                currentPageLogic.RunLogic(landmarkManager.GetHandPoints(), landmarkManager.GetFacePoints());

            if (currentPageLogic.IsPageDone())
            {
                sceneAnimationEnd.PlayUIAnim();
                //NextPage();
            }
        }

        // If gesture navigation detected
        if (gestureNavigationDetected)
        {
            nextPageTimeElapsed += Time.deltaTime;

            if (nextPageTimeElapsed >= nextPageTime)
            {
                Debug.Log("next page");
                nextPageTimeElapsed = 0f;
                PreNextPage();
            }
        }
        else if (nextPageTimeElapsed > 0)
        {
            nextPageTimeElapsed -= Time.deltaTime;
        }
        nextPageTimeElapsed = Mathf.Clamp(nextPageTimeElapsed, 0, nextPageTime);
        if(img_nextPageFilled)
        img_nextPageFilled.fillAmount = nextPageTimeElapsed / nextPageTime;

    }


    public void StoreFacePoints(List<GameObject> facePoints)
    {
        //this.facePoints = facePoints;
        foreach (var pageL in pageLogics)
            pageL.LoadFacePoints(true);

    }
    public void StoreHandPoints(List<HandPointAnnotationObject> handPoints)
    {
        //this.handPoints = handPoints;
        foreach (var item in handPoints)
            item.OnCollidingFacePoint += OnHandPointCollide;

        handPointsStored = true;

        if (!currentPageLogic.HandpointsLoaded)
        {
            foreach (var item in handPoints)
                item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
            currentPageLogic.LoadHandPoints(true);
        }
    }
    public void OnHandPointCollide(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
        //Debug.Log(handPointIndex + " face: " + facePointIndex);
    }
    void StartPage()
    {
        sceneAnimationStart.PlayUIAnim();
        currentPageLogic = pageLogics[currentPageIndex];
        currentPageLogic.gameObject.SetActive(true);

        img_splashIcon.sprite = currentPageLogic.pageData.spr_splashIcon;
        tmp_description.text = currentPageLogic.pageData.splashDescription;

        // enable face / hand traking
        annotationFace.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
        annotationHand.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);
        // enable header text
        img_face.transform.parent.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
        img_hand.transform.parent.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);

        if (currentPageLogic.pageData.enableSplashPage)
            splashScreen.ShowSplash();
        else
            splashScreen.gameObject.SetActive(false);
        headertransform.gameObject.SetActive(currentPageLogic.pageData.enableHeader);


        currentPageLogic.StartPage();
        currentPageLogic.ChangeState(PageLogic.PageState.START);

        if (handPointsStored && !currentPageLogic.HandpointsLoaded)
        {
            foreach (var item in landmarkManager.GetHandPoints())
                item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
            currentPageLogic.LoadHandPoints(true);
        }
    }
    void PreNextPage()
    {
        if (currentPageIndex < pageLogics.Count-1)
            sceneAnimationEnd.PlayUIAnim();

    }
    void NextPage()
    {

        currentPageLogic.EndPage();
        currentPageLogic.gameObject.SetActive(false);
        currentPageIndex++; // increase page index
        sceneAnimationStart.OnAnimationFinished = null;
        currentPageLogic.ChangeState(PageLogic.PageState.ENDED);
        StartPage();
    }

    // GESTURES
    void OnGestureFirstDetected(string gesture)
    {
        if (handGestureManager.IsGestureDetected()) return;

        Debug.Log("New gesture detected");
            if (gestureNavigation.gestureName == gesture)
                gestureNavigationDetected = true;
            else
            gestureNavigationDetected = false;

    }
    void OnGestureDetecting(string gesture)
    {
        if (!handGestureManager.IsGestureDetected()) return;
            Debug.Log("gesture detected " + gesture);
            if (gestureNavigation.gestureName == gesture)
            {
                gestureNavigationDetected = true;
            }
            else
            {
                gestureNavigationDetected = false;
            }
    }
}

