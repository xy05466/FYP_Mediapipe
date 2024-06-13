using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Add your singleton references here
    public SceneManagement sceneManagement { get; private set; }
    public GetSceneManager getSceneManager { get; private set; }
    public static bool debugMode = false;
    static bool CanClick;
    private void Start()
    {
        sceneManagement = SceneManagement.Instance;
        getSceneManager = GetComponent<GetSceneManager>();
        getSceneManager.Init();
    }

}