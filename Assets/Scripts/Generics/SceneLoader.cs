using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image image;
    public Animator transition;

    public float raycastTime = 2.5f;
    public float transitionTime = 1f;

    private void Start()
    {
        StartCoroutine(SetRaycastTarget());
    }

    public void StartSetRaycastTarget()
    {
        StartCoroutine(SetRaycastTarget());
    }

    IEnumerator SetRaycastTarget()
    {
        yield return new WaitForSeconds(raycastTime);
        image.raycastTarget = false;
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        image.raycastTarget = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
