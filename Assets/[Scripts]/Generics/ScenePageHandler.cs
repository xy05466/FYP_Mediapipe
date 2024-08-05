using Mediapipe.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static FaceHandPoint;

// Will handle the pages and splash screen pop up etc.
// Basically all the common things in a scene
public class ScenePageHandler : MonoBehaviour
{
    [Header("Managers")]
    public LandmarkManager landmarkManager;
    // temp list
    public List<PageLogic> pageLogics;
    private PageLogic currentPageLogic;
    int currentPageIndex = 0;
    //public HandGestureManager handGestureManager;


    //[Header("Splash")]
    //public GameObject splashArt;
    //public Button bu_closeSplashArt;
    [Header("Splash Page")]
    public SplashScreen splashScreen;
    public UISpriteAnimation sceneAnimationStart;
    public UISpriteAnimation sceneAnimationEnd;
    public Transform headertransform;
    public TMP_Text tmp_title;
    public TMP_Text tmp_titleSub;
    public Image img_splashIcon;
    public TMP_Text tmp_description;

    [Header("Hand Point Collision")]
    public float distanceCollision = 0.2f;
    //public Image img_face;
    //public Image img_hand;
    [Header("Pages")]
    private bool gestureNavigationDetected = false;
    //public Button bu_prevPage;
    //public Button bu_nextPage;
    private bool autoNavigate = false;
    private bool pageEnded = false;
    //[Header("Gesture Navigation")]
    //public GestureSO gestureNavigation;
    //public Image img_nextPageFilled;
    //public GameObject panel_gesture;
    //public float nextPageTime = 3f;
    //private float nextPageTimeElapsed;

    [Header("Annotation screens")]
    //public bool isFaceDetectionTask = true;
    //public bool isHandDetectionTask = true;
    public GameObject annotationFace;
    //public GameObject annotationFace_task;
    public GameObject annotationHand;
    //public GameObject annotationHand_task;

    [Header("Destection Solution")]
    //public GameObject faceSolution;
    ////public GameObject faceSolution_task;
    //public GameObject handSolution;
    //public GameObject handSolution_task;
    //[Header("Annotation List of Points")]
    //public LandmarkListGO annotationList_FacePoints;
    //public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints;
    //List<GameObject> facePoints = new List<GameObject>();
    //List<HandPointAnnotationObject> handPoints = new List<HandPointAnnotationObject>();
    public System.Action<int, int, GameObject, int, GameObject> OnCollidingFacePoint; // hand point collide with face point


    bool handPointsStored = false;
    private void Start()
    {

        annotationFace.gameObject.SetActive(false);
        //annotationFace_task.gameObject.SetActive(false);
        annotationHand.gameObject.SetActive(false);
        //annotationHand_task.gameObject.SetActive(false);

        //faceSolution.gameObject.SetActive(false);
        //faceSolution_task.gameObject.SetActive(false);
        //handSolution.gameObject.SetActive(false);
        //handSolution_task.gameObject.SetActive(false);

        // Check face detection type
        //if (isFaceDetectionTask)
        //{
        //    annotationFace_task.gameObject.SetActive(true);
        //    faceSolution.gameObject.SetActive(false);
        //    faceSolution_task.gameObject.SetActive(true);
        //}
        //else
        //{
        //    annotationFace.gameObject.SetActive(true);
        //    faceSolution_task.gameObject.SetActive(false);
        //    faceSolution.gameObject.SetActive(true);

        //}

        //// Check hand detection type
        //if (isHandDetectionTask)
        //{
        //    annotationHand_task.gameObject.SetActive(true);
        //    handSolution.gameObject.SetActive(false);
        //    handSolution_task.gameObject.SetActive(true);
        //}
        //else
        //{
        //    annotationHand.gameObject.SetActive(true);
        //    handSolution_task.gameObject.SetActive(false);
        //    handSolution.gameObject.SetActive(true);
        //}

        landmarkManager.Initialize();


        //if (panel_gesture!=null)
        //    panel_gesture.gameObject.SetActive(false);

        //bu_nextPage?.onClick.AddListener(() => { PreNextPage(); });
        // go to next page after scene animation
        sceneAnimationEnd.OnAnimationFinished += NextPage;


        landmarkManager.SubscribeFacePointsActive(OnFaceSpotted);
        landmarkManager.SubscribeHandPointsActive(OnHandSpotted);

        landmarkManager.SubscribeFacePointsLoaded(StoreFacePoints);
        landmarkManager.SubscribeHandPointsLoaded_1(StoreHandPoints);
        landmarkManager.SubscribeHandPointsLoaded_2(StoreHandPoints);
        for (int i = 0; i < pageLogics.Count; i++)
        {
            PageLogic_Extension pageLogic_Extension = pageLogics[i] as PageLogic_Extension;
            if(pageLogic_Extension!= null)
            {
                landmarkManager.SubscribeFacePointsActive(pageLogic_Extension.OnFaceAnnotationActive);
                landmarkManager.SubscribeHandPointsActive(pageLogic_Extension.OnHandAnnotationActive);
            }
        }

        //// subscribe gesture events
        //handGestureManager.SubscribeFirstDetected(OnGestureFirstDetected);
        //handGestureManager.SubscribeDetecting(OnGestureDetecting);

   

        // Disable all pages
        for (int i = 0; i < pageLogics.Count; i++)
            pageLogics[i].gameObject.SetActive(false);

        for (int i = 0; i < pageLogics.Count; i++)
            pageLogics[i].Initialize();

        //currentPageLogic = pageLogics[currentPageIndex];
        StartPage();
    }
    private void Update()
    {
        landmarkManager.UpdateLandmarks();
        if (!sceneAnimationEnd.IsDone()) return;
        // update hand face point collision
        if (landmarkManager.IsFaceloadedAndActive)
        {
            for (int j = 0; j < landmarkManager.GetFacePoints().Count; j++)
            {
                if (landmarkManager.IsHandLoadedAndActive_1)
                {
                    for (int i = 0; i < landmarkManager.GetHandPoints(0).Count; i++)
                    {
                        HandPointAnnotationObject handPoint = landmarkManager.GetHandPoints(0)[i];
                        GameObject facePoint = landmarkManager.GetFacePoints()[j];
                        if (Vector2.Distance(handPoint.transform.position, facePoint.transform.position) <= distanceCollision)
                        {
                            OnCollidingFacePoint?.Invoke(0,i, handPoint.gameObject, j, facePoint);
                        }
                    }
                }

                if (landmarkManager.IsHandLoadedAndActive_2)
                {
                    for (int i = 0; i < landmarkManager.GetHandPoints(1).Count; i++)
                    {
                        HandPointAnnotationObject handPoint = landmarkManager.GetHandPoints(1)[i];
                        GameObject facePoint = landmarkManager.GetFacePoints()[j];
                        if (Vector2.Distance(handPoint.transform.position, facePoint.transform.position) <= distanceCollision)
                        {
                            OnCollidingFacePoint?.Invoke(1, i, handPoint.gameObject, j, facePoint);
                        }
                    }
                }
            }
        }


        if (currentPageLogic != null)
        {
            // run curring page logic
            if (currentPageLogic.CompareState(PageLogic.PageState.START))
                currentPageLogic.RunLogic(landmarkManager.GetHandPoints(0), landmarkManager.GetHandPoints(1), landmarkManager.GetFacePoints());

            if (currentPageLogic.IsPageDone() && !pageEnded)
            {
                pageEnded = true;
                if(autoNavigate)
                    sceneAnimationEnd.PlayUIAnim();
            }
        }

        #region Gesture code
        //// If gesture navigation detected
        //if (handGestureManager.IsGestureDetected() && handGestureManager.GetCurrentGesture() == gestureNavigation.gestureName)
        //{
        //    if (!autoNavigate)
        //        panel_gesture.gameObject.SetActive(false);

        //    nextPageTimeElapsed += Time.deltaTime;
        //    if (nextPageTimeElapsed >= nextPageTime)
        //    {
        //        Debug.Log("next page");
        //        nextPageTimeElapsed = 0f;
        //        PreNextPage();
        //    }

        //}
        //else if (nextPageTimeElapsed > 0)
        //{
        //    nextPageTimeElapsed -= Time.deltaTime;
        //    if (!autoNavigate && pageEnded)
        //        panel_gesture.gameObject.SetActive(true);
        //}
        //else
        //{
        //    if (!autoNavigate && pageEnded)
        //        panel_gesture.gameObject.SetActive(true);
        //}
        //nextPageTimeElapsed = Mathf.Clamp(nextPageTimeElapsed, 0, nextPageTime);
        //if (img_nextPageFilled)
        //    img_nextPageFilled.fillAmount = nextPageTimeElapsed / nextPageTime;
        #endregion
    }
    private void OnFaceSpotted(bool active)
    {
        //img_face.gameObject.SetActive(active);
    }
    private void OnHandSpotted(int actives)
    {
        //img_hand.gameObject.SetActive(active);
    }
    public void StoreFacePoints(List<GameObject> facePoints)
    {
        //this.facePoints = facePoints;
        foreach (var pageL in pageLogics)
            pageL.LoadFacePoints(true);

    }
    public void StoreHandPoints(List<HandPointAnnotationObject> handPoints)
    {

        foreach (var pageL in pageLogics)
            pageL.LoadHandPoints(true);

        ////this.handPoints = handPoints;
        //foreach (var item in handPoints)
        //    item.OnCollidingFacePoint += OnHandPointCollide;

        handPointsStored = true;

        //foreach (var item in handPoints)
        //    item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;

        OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;


        //foreach (var pageL in pageLogics)
        //{
        //    //foreach (var item in handPoints)
        //    //    item.OnCollidingFacePoint += pageL.OnHandPointCollideFacePoint;

        //    OnCollidingFacePoint += pageL.OnHandPointCollideFacePoint;
        //}
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
        //if(isFaceDetectionTask)
        //    annotationFace_task.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
        //else
            annotationFace.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);

        //if(isHandDetectionTask)
        //    annotationHand_task.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);
        //else
            annotationHand.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);
        //// enable header icons
        //img_face.transform.parent.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
        //img_hand.transform.parent.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);

        // set up title
        tmp_title.text = currentPageLogic.pageData.title;
        tmp_titleSub.text = currentPageLogic.pageData.titleSub;
        autoNavigate = currentPageLogic.pageData.autoNavigate;
        if (currentPageLogic.pageData.enableSplashPage)
            splashScreen.ShowSplash();
        else
            splashScreen.gameObject.SetActive(false);
        headertransform.gameObject.SetActive(currentPageLogic.pageData.enableHeader);

        // start page for current page
        currentPageLogic.StartPage();
        currentPageLogic.ChangeState(PageLogic.PageState.START);

        //// subscribe hand points events to page
        //if (handPointsStored && !currentPageLogic.HandpointsLoaded)
        //{
        //    foreach (var item in landmarkManager.GetHandPoints())
        //        item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
        //}

        OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
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
        pageEnded = false;

        //if(landmarkManager.IsHandLoaded)
        //{
        //    foreach (var item in landmarkManager.GetHandPoints())
        //        item.OnCollidingFacePoint -= currentPageLogic.OnHandPointCollideFacePoint;

        //    OnCollidingFacePoint -= currentPageLogic.OnHandPointCollideFacePoint;
        //}
        if(landmarkManager.NumHandsLoaded>0)
            OnCollidingFacePoint -= currentPageLogic.OnHandPointCollideFacePoint;
        StartPage();
    }

    // GESTURES
    void OnGestureFirstDetected(string gesture)
    {
        //if ()) return;

        //Debug.Log("New gesture detected");
        //    if (gestureNavigation.gestureName == gesture)
        //        gestureNavigationDetected = true;
        //    else
        //    gestureNavigationDetected = false;

    }
    void OnGestureDetecting(string gesture)
    {
        //if (!handGestureManager.IsGestureDetected()) return;
        //    Debug.Log("gesture detected " + gesture);
        //    if (gestureNavigation.gestureName == gesture)
        //    {
        //        gestureNavigationDetected = true;
        //    }
        //    else
        //    {
        //        gestureNavigationDetected = false;
        //    }
    }
}

