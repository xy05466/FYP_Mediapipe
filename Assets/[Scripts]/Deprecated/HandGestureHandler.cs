using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGestureHandler : MonoBehaviour
{
    [SerializeField] private string currGesture;


    //public float gestureTime = 3.0f;

    // For switching to next scene
    //public SceneLoader sceneLoader;
    //public string nextScene;

    //public Action<string> OnHandGestureDetected;
    //public bool isDetected = true;
    //public Action OnHandGesturePeaceDetected;
    // Do-Later: Integrate massage scene demo with gestures

    //private void Update()
    //{
    //    HandleGestures(currGesture);
    //}
    public void SetCurrentGesture(string gesture)
    {
        currGesture = gesture;
    }
    ////handle inputs here
    ////maybe can use event system
    //void HandleGestures(string gesture)
    //{
    //    OnHandGestureDetected?.Invoke(gesture);
    //    //switch (gesture)
    //    //{
    //    //    case "Peace":
    //    //        // example: play a demo for the massage scene
    //    //        OnHandGestureDetected?.Invoke(gesture);
    //    //        break;
    //    //    case "OK":
    //    //        OnHandGestureDetected?.Invoke(gesture);
    //    //        break;
    //    //    default:
    //    //        Debug.Log("Invalid choice.");
    //    //        break;
    //    //}
    //}
}
