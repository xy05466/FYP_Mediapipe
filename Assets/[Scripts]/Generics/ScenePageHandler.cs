using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public LandmarkListAnnotation_Extension annotationList_FacePoints;
    public LandmarkListAnnotation_Extension annotationList_HandPoints;
    List<GameObject> facePoints = new List<GameObject>();
    List<GameObject> handPoints = new List<GameObject>();

    PageData currentPage;
    PageLogic currentPageLogic;
    private void Start()
    {
        annotationList_FacePoints.OnHandpointsSpawned += StoreFacePoints;
        annotationList_HandPoints.OnHandpointsSpawned += HandFacePoints;
        StartPage();
    }
    public void StoreFacePoints(List<GameObject> facePoints)
    {
        this.facePoints = facePoints;
    }
    public void HandFacePoints(List<GameObject> handPoints)
    {
        this.handPoints = handPoints;
    }
    void StartPage()
    {
        currentPage = pageDatas[currentPageIndex];
        currentPageLogic = pageLogics[currentPageIndex];
        img_splashIcon.sprite = currentPage.spr_splashIcon;
        tmp_description.text = currentPage.splashDescription;

        //bu_closeSplashArt.onClick.AddListener(() => { splashArt.SetActive(false); });
        annotationFace.gameObject.SetActive(currentPage.enableFaceTracking);
        annotationHand.gameObject.SetActive(currentPage.enableHandTracking);
        splashTransform.gameObject.SetActive(currentPage.enableSplashPage);

        currentPageLogic.StartPage();
    }

    private void Update()
    {
        if(currentPageLogic != null)
        {
            currentPageLogic.RunLogic();

            if (currentPageLogic.IsPageDone())
            {
                NextPage();
            }
        }
    }
    void NextPage()
    {
        currentPageLogic.EndPage();
        currentPageIndex++;
        if(currentPageIndex < pageDatas.Count - 1)
        {
            StartPage();
        }
    }
}
