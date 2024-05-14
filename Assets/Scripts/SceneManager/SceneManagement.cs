using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
//Created By: Tan Xiang Feng Wayne
public class SceneManagement : MonoBehaviour
{
    private static SceneManagement instance = null;
    public static SceneManagement Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject newGO = Instantiate(Resources.Load("Prefabs/SceneManager") as GameObject, null);
                newGO.name = "SceneManager";
                newGO.SetActive(true);
                DontDestroyOnLoad(newGO);
                //call event here to init 
            }
            return instance;
        }
    }

    private AsyncOperation asyncHandler = null;
    private Coroutine loadCoroutine = null;

    [Header("Canvas")]
    public TMPro.TMP_Text loadingText;

    public bool isLoading { get; private set; }
    public bool isLoadingEffect { get; private set; }

    public Animator transition;
    public float transitionTime = 1f;

    private System.Action onSceneLoaded;

    [Header("CSVReader")]
    private CSVReader cSVReader;
    //public Volume dissolveVolume;

    public void OnSceneLoaded(System.Action onSceneLoad)
    {
        if (isLoading)
            onSceneLoaded += onSceneLoad;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        instance = this;
        loadingText.gameObject.SetActive(false);
        cSVReader = GetComponent<CSVReader>();
        cSVReader.ReadCSV();
    }

    public void LoadScene(string name)
    {
        if (loadCoroutine == null)
            loadCoroutine = StartCoroutine(LoadSceneRoutine(name));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;
        isLoadingEffect = true;
        Time.timeScale = 0;
        //Doing Scene Transitions

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        asyncHandler = SceneManager.LoadSceneAsync(sceneName);
        loadingText.text = "Loading... 0%";
        loadingText.gameObject.SetActive(true);

        if (asyncHandler == null)
            yield break;

        //When the async progress is not done
        //while (!asyncHandler.isDone)
        //{
        //    //transition.Play("petalleave");
        //    Debug.Log(string.Format("Load Scene {0}  Progress: {1:P2}", sceneName, asyncHandler.progress));
        //    loadingText.text = string.Format("Loading... {0:P2}", asyncHandler.progress);
        //    yield return null;
        //}
        //transition.Play("petalenter");
        asyncHandler = null;
        onSceneLoaded?.Invoke();
        onSceneLoaded = null;
        loadingText.gameObject.SetActive(false);
        isLoading = false;
        loadCoroutine = null;
        

        //Default set game time scale to 1 when a new scene is loaded
        Time.timeScale = 1;
        isLoadingEffect = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnSceneLoadSetTimeScale(float timeScale)
    {
        onSceneLoaded += () => Time.timeScale = timeScale;
    }

    public string GetActiveSceneName() => SceneManager.GetActiveScene().name;
}
