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
    public List<PageData> pageData;
    int currentPaageIndex = 0;

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

    [Header("Annotations")]
    public GameObject annotationFace;
    public GameObject annotationHand;

    private void Start()
    {
        PageData currentPage = pageData[currentPaageIndex];
        img_splashIcon.sprite = currentPage.spr_splashIcon;
        tmp_description.text = currentPage.splashDescription;

        //bu_closeSplashArt.onClick.AddListener(() => { splashArt.SetActive(false); });
        annotationFace.gameObject.SetActive(currentPage.enableFaceTracking);
        annotationHand.gameObject.SetActive(currentPage.enableHandTracking);
        splashTransform.gameObject.SetActive(currentPage.enableSplashPage);
    }
}
