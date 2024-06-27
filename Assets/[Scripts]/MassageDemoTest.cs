using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassageDemoTest : MonoBehaviour
{
    public HandGestureHandler handGestureHandler;

    [SerializeField] private float elapsedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (handGestureHandler != null)
        {
            handGestureHandler.OnHandGestureDetected += DetectGesture;
        }
    }
    public void DetectGesture(string gesture)
    {
        // when gesture is activated, start a timer to run for around ~3 secs
        if (handGestureHandler.isDetected && gesture == "Peace")
        {
            elapsedTime += Time.deltaTime;
            // To-do: Timer must only run once, so we need a boolean variable to detect if this thing should run or not
            // if the function is running, this condition should be false
            if (elapsedTime >= handGestureHandler.gestureTime)
            {
                // Reset timer 
                elapsedTime = 0;
                Debug.Log("Currently playing demo for massage scene");
                handGestureHandler.isDetected = false;
            }
        }
    }

}
