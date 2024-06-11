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
    //public List<string> sceneToLoad;
    public List<PageData> pageDatas;
    // temp list
    public List<PageLogic> pageLogics;
    int currentPageIndex = 0;

    //[Header("Splash")]
    //public GameObject splashArt;
    //public Button bu_closeSplashArt;
    [Header("Splash Page")]
    public UISpriteAnimation sceneAnimation;
    public Transform splashTransform;
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

    PageData currentPage;
    PageLogic currentPageLogic;

    private void Start()
    {
        bu_nextPage.onClick.AddListener(PreNextPage);

        annotationList_FacePoints.OnPointsSpawned += StoreFacePoints;
        annotationList_HandPoints.OnPointsSpawned += StoreHandPoints;
        StartPage();
    }

    void PreNextPage()
    {
        sceneAnimation.PlayUIAnim(false);
        // go to next page after scene animation
        sceneAnimation.OnAnimationFinished += NextPage;
    }

    public void StoreFacePoints(List<GameObject> facePoints)
    {
        this.facePoints = facePoints;
        foreach (var pageL in pageLogics)
            pageL.loadedFacepoints = true;

    }
    bool handPointsStored = false;
    public void StoreHandPoints(List<HandPointAnnotationObject> handPoints)
    {
        this.handPoints = handPoints;
        foreach (var item in handPoints)
            item.OnCollidingFacePoint += OnHandPointCollide;
        handPointsStored = true;

        if (!currentPageLogic.loadedHandponts)
        {
            foreach (var item in handPoints)
                item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
            currentPageLogic.loadedHandponts = true;
        }
    }
    public void OnHandPointCollide(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
        //Debug.Log(handPointIndex + " face: " + facePointIndex);
    }
    void StartPage()
    {
        sceneAnimation.PlayUIAnim();
        currentPage = pageDatas[currentPageIndex];
        currentPageLogic = pageLogics[currentPageIndex];
        img_splashIcon.sprite = currentPage.spr_splashIcon;
        tmp_description.text = currentPage.splashDescription;

        //bu_closeSplashArt.onClick.AddListener(() => { splashArt.SetActive(false); });
        annotationFace.gameObject.SetActive(currentPage.enableFaceTracking);
        annotationHand.gameObject.SetActive(currentPage.enableHandTracking);
        splashTransform.gameObject.SetActive(currentPage.enableSplashPage);

        currentPageLogic.StartPage();
        if (handPointsStored && !currentPageLogic.loadedHandponts)
        {
            foreach (var item in handPoints)
                item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
            currentPageLogic.loadedHandponts = true;
        }
    }

    private void Update()
    {
        annotationList_FacePoints.Update();
        annotationList_HandPoints.Update();
        if (currentPageLogic != null)
        {
            currentPageLogic.RunLogic(handPoints, facePoints);

            if (currentPageLogic.IsPageDone())
            {
                NextPage();
            }
        }
    }
    void NextPage()
    {
        sceneAnimation.OnAnimationFinished = null;
        currentPageLogic.EndPage();
        currentPageIndex++;
        if(currentPageIndex < pageDatas.Count - 1)
        {
            StartPage();
        }
     
      
    }
}
