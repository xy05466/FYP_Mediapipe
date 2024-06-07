using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Will handle the pages and splash screen pop up etc.
// Basically all the common things in a scene
public class ScenePageHandler : MonoBehaviour
{
    //public List<string> sceneToLoad;
    public List<PageData> pageData;

    //[Header("Splash")]
    //public GameObject splashArt;
    //public Button bu_closeSplashArt;


    [Header("Pages")]
    public Button bu_prevPage;
    public Button bu_nextPage;

    private void Start()
    {
        //bu_closeSplashArt.onClick.AddListener(() => { splashArt.SetActive(false); });
    }
}
