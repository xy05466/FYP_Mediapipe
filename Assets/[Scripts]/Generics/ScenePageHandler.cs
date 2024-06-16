using Mediapipe.Unity;
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


    [Header("Pages")]
    public Button bu_prevPage;
    public Button bu_nextPage;

    [Header("Annotation screens")]
    public GameObject annotationFace;
    public GameObject annotationHand;
    [Header("Annotation List of Points")]
    public LandmarkListGO annotationList_FacePoints;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints;
    List<GameObject> facePoints = new List<GameObject>();
    List<HandPointAnnotationObject> handPoints = new List<HandPointAnnotationObject>();


    bool handPointsStored = false;
    private void Start()
    {
        bu_nextPage.onClick.AddListener(() => { PreNextPage(); });
        // go to next page after scene animation
        sceneAnimationEnd.OnAnimationFinished += NextPage;

        annotationList_FacePoints.OnPointsSpawned += StoreFacePoints;
        annotationList_HandPoints.OnPointsSpawned += StoreHandPoints;

        for (int i = 0; i < pageLogics.Count; i++)
        {
            PageLogic_Extension pageLogic_Extension = pageLogics[i] as PageLogic_Extension;
            if(pageLogic_Extension!= null)
            {
                annotationList_FacePoints.OnParentActive += pageLogic_Extension.OnFaceAnnotationActive;
                annotationList_HandPoints.OnParentActive += pageLogic_Extension.OnHandAnnotationActive;
            }
        }


        for (int i = 0; i < pageLogics.Count; i++)
            pageLogics[i].gameObject.SetActive(false);


        StartPage();
    }
    private void Update()
    {
        if (!sceneAnimationEnd.IsDone()) return;
        // find annotation points
        annotationList_FacePoints.Update();
        annotationList_HandPoints.Update();

        if (currentPageLogic != null)
        {
            // run curring page logic
            if (currentPageLogic.CompareState(PageLogic.PageState.START))
                currentPageLogic.RunLogic(handPoints, facePoints);

            if (currentPageLogic.IsPageDone())
            {
                sceneAnimationEnd.PlayUIAnim();
                //NextPage();
            }
        }
    }
 

    public void StoreFacePoints(List<GameObject> facePoints)
    {
        this.facePoints = facePoints;
        foreach (var pageL in pageLogics)
            pageL.LoadFacePoints(true);

    }
    public void StoreHandPoints(List<HandPointAnnotationObject> handPoints)
    {
        this.handPoints = handPoints;
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

        //bu_closeSplashArt.onClick.AddListener(() => { splashArt.SetActive(false); });
        annotationFace.gameObject.SetActive(currentPageLogic.pageData.enableFaceTracking);
        annotationHand.gameObject.SetActive(currentPageLogic.pageData.enableHandTracking);
        if (currentPageLogic.pageData.enableSplashPage)
            splashScreen.ShowSplash();
        headertransform.gameObject.SetActive(currentPageLogic.pageData.enableHeader);


        currentPageLogic.StartPage();
        currentPageLogic.ChangeState(PageLogic.PageState.START);

        if (handPointsStored && !currentPageLogic.HandpointsLoaded)
        {
            foreach (var item in handPoints)
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
}

