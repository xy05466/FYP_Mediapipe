using System;
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
    [SerializeField] private float elapsedTime = 0;

    //private Scene currentScene;
    public HandGestureHandler handGestureHandler;

    private void Start()
    {
        StartCoroutine(SetRaycastTarget());
        //currentScene = SceneManager.GetActiveScene();
        //Debug.Log(currentScene.name);
        // To-Do: subscribe to OnHandGestureDetected
        if (handGestureHandler != null)
        {
            handGestureHandler.OnHandGestureDetected += DetectGesture;
        } else
        {
            Debug.Log("HandGestureHandler is missing!");
        }
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

    public void DetectGesture(string gesture)
    {
        // when gesture is activated, start a timer to run for around ~3 secs
        if (handGestureHandler.isDetected && gesture == "OK")
        {
            elapsedTime += Time.deltaTime;
            // To-do: Timer must only run once, so we need a boolean variable to detect if this thing should run or not
            // if the function is running, this condition should be false
            if (elapsedTime >= handGestureHandler.gestureTime)
            {
                // Reset timer 
                elapsedTime = 0;
                LoadNextLevel(handGestureHandler.nextScene);
                handGestureHandler.isDetected = false;
            }
        }
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
