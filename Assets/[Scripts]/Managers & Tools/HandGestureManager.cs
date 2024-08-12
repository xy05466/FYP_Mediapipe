using Mediapipe.Tasks.Vision.HandLandmarker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;
//using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using System;
using System.Linq;
public class HandGestureManager : MonoBehaviour
{
    [SerializeField] private LandmarkManager landmarkManager;
    private string prevGesture;
    [SerializeField] private string currGesture;

    [SerializeField] private List<GestureData> gestures;

    private Action<string> OnDetecting;
    private Action<string> OnFirstDetected;

    private const string NO_GESTURE_STRING = "NA";
    private void Start()
    {
        landmarkManager.SubscribeHandPointsActive(OnHandPointsActive);
    }

    private void Update()
    {
        if (landmarkManager.GetHandPoints(0) == null || landmarkManager.GetHandPoints(0).Count <= 0 || !landmarkManager.GetHandPoints(0)[0].gameObject.activeInHierarchy)
        {
            currGesture = NO_GESTURE_STRING;
            return;
        }
        ReadGesture(landmarkManager.GetHandPoints(0).ToArray());
    }

    private void OnHandPointsActive(int numHandsActive)
    {
        // if not hands active
        if (numHandsActive <= 0)
            currGesture = "NA";
    }
    private string ReadGesture(HandPointAnnotationObject[] landmarkList)
    {
        // define finger tips
        GameObject handBase = landmarkList[0].gameObject;
        GameObject thumbTip = landmarkList[4].gameObject;
        GameObject indexTip = landmarkList[8].gameObject;
        GameObject middleTip = landmarkList[12].gameObject;
        GameObject ringTip = landmarkList[16].gameObject;
        GameObject pinkyTip = landmarkList[20].gameObject;


        GameObject middleBase = landmarkList[9].gameObject;

        float distancehandBase = Vector3.Distance(middleBase.transform.position, handBase.transform.position);
        // tip distances to base of hand
        var thumbDist = CheckDistance(handBase, thumbTip) / distancehandBase;
        var indexDist = CheckDistance(handBase, indexTip) / distancehandBase;
        var middleDist = CheckDistance(handBase, middleTip) / distancehandBase;
        var ringDist = CheckDistance(handBase, ringTip) / distancehandBase;
        var pinkyDist = CheckDistance(handBase, pinkyTip) / distancehandBase;
        List<float> fingerDistances = new List<float>() { thumbDist, indexDist, middleDist, ringDist, pinkyDist };
        float CurrHighestPercentage = 0f;
        string CurrHighestName = NO_GESTURE_STRING;
        //need a list to keep track of all teh gestures
        for (int i = 0; i < gestures.Count; i++)
        {
            float gesturePercentage = CheckGesturePercentage(gestures[i], fingerDistances);
            //Debug.Log(gestures[i].gestureName + " " + gesturePercentage);

            if (gesturePercentage > CurrHighestPercentage)
            {
                //CurrHighestName = gestures[i].gestureName;
                CurrHighestPercentage = gesturePercentage;
            }
            if (gesturePercentage >= 90)
            {
                currGesture = gestures[i].gestureName;
                CurrHighestName = gestures[i].gestureName;
            }
        }
        if (CurrHighestPercentage == 0)
        {
            CurrHighestName = NO_GESTURE_STRING;
        }

        //This to to update the current gesture being detected
        //gestureText.UpdateTextBox(CurrHighestName);
        currGesture = CurrHighestName;

        // on first gesture detected
        if (currGesture != prevGesture)
            OnFirstDetected?.Invoke(currGesture);

        // on gesture detecting
        OnDetecting?.Invoke(currGesture);

        // updaete prevGesture
        prevGesture = currGesture;

        return currGesture;
    }
    // Check distance between point a and n,
    // to check the distance between hand points
    private float CheckDistance(GameObject a, GameObject b)
    {

        float distX, distY, distZ;
        distX = a.transform.position.x - b.transform.position.x;
        distY = a.transform.position.y - b.transform.position.y;
        distZ = 0;
        var newVec = new Vector3(distX, distY, distZ);
        return Vector3.Magnitude(newVec);
    }
    // Read which gesture is detecting

    private float CheckGesturePercentage(GestureData gesture, List<float> fingerDistances)
    {
        float currGesturePercentage = 0;

        // check for each finger
        for (int i = 0; i < fingerDistances.Count; i++)
        {
            //Debug.Log("index " + i + " " + fingerDistances[i]);
            if (fingerDistances[i] >= gesture.fingerDistances[i].minDistance && fingerDistances[i] <= gesture.fingerDistances[i].minDistance+ gesture.fingerDistances[i].threshold)
            {
                currGesturePercentage += 20;
            }
        }
        return currGesturePercentage;
    }
   
    // Get current gesture
    public string GetCurrentGesture() => currGesture;

    // If gesture is detecting
    public bool IsGestureDetected() => currGesture != NO_GESTURE_STRING || NO_GESTURE_STRING != "";

    // SUBSCRIBE EVENTS
    // EVENT : First Detected
    public void SubscribeFirstDetected(Action<string> action)=> OnFirstDetected += action;
    public void UnsubscribeFirstDetected(Action<string> action)=> OnFirstDetected -= action;

    // EVENT : Detecting
    public void SubscribeDetecting(Action<string> action) => OnDetecting += action;
    public void UnsubscribeDetecting(Action<string> action) => OnDetecting -= action;

    internal void NoGesture() => currGesture = NO_GESTURE_STRING;
}
