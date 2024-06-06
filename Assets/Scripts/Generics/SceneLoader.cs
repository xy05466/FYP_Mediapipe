using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image image;
    public Animator transition;
    public UISpriteAnimation sceneAnimation;

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
    string sceneName;
    public void LoadNextLevel(string sceneName)
    {
        if (sceneAnimation)
            sceneAnimation.PlayUIAnim(!sceneAnimation.m_reverse);
        this.sceneName = sceneName;

        sceneAnimation.OnAnimationFinished += TransitionEnded;
    }
    public void TransitionEnded()
    {
        sceneAnimation.OnAnimationFinished -= TransitionEnded;
        SceneManager.LoadScene(sceneName);
    }
    //IEnumerator LoadLevel(string sceneName)
    //{
    //    image.raycastTarget = true;
    //    if (transition)
    //        transition.SetTrigger("Start");
    //    if(sceneAnimation)
    //        sceneAnimation.PlayUIAnim(!sceneAnimation.m_reverse);
    //    yield return new WaitForSeconds(transitionTime);
    //    SceneManager.LoadScene(sceneName);
    //}
}
