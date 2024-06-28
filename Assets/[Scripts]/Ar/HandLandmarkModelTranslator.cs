using System.Collections.Generic;
using UnityEngine;

//using System.Diagnostics;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;

//namespace Mediapipe.Unity.Sample.HandLandmarkDetection
public class HandLandmarkModelTranslator : MonoBehaviour
{
    public GestureText gestureText;
    public CoordText coordText;
    public PercentageText percentageText;
    public List<Gesture> gestures;
    public HandGestureHandler handGestureHandler = null;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float minimumTreshold = 50;
    [Range(0.0f, 0.1f)]
    [SerializeField] private float tolerance = 0.0f;
    [Range(0.0f, 0.6f)]
    [SerializeField] private float rangeFallOff = 0.0f;

    private float CheckDistance(Landmark a, Landmark b)
    {
        float distX, distY, distZ;
        distX = a.x - b.x;
        distY = a.y - b.y;
        distZ = a.z - b.z;
        var newVec = new Vector3(distX, distY, distZ);
        return Vector3.Magnitude(newVec);
    }

    public string ReadGesture(HandLandmarkerResult landmarkList)
    {
        // define finger tips
        var handBase = landmarkList.handWorldLandmarks[0].landmarks[0];
        var thumbTip = landmarkList.handWorldLandmarks[0].landmarks[4];
        var indexTip = landmarkList.handWorldLandmarks[0].landmarks[8];
        var middleTip = landmarkList.handWorldLandmarks[0].landmarks[12];
        var ringTip = landmarkList.handWorldLandmarks[0].landmarks[16];
        var pinkyTip = landmarkList.handWorldLandmarks[0].landmarks[20];
        // tip distances to base of hand
        var thumbDist = CheckDistance(handBase, thumbTip);
        var indexDist = CheckDistance(handBase, indexTip);
        var middleDist = CheckDistance(handBase, middleTip);
        var ringDist = CheckDistance(handBase, ringTip);
        var pinkyDist = CheckDistance(handBase, pinkyTip);
        //Debug.Log(thumbDist + " " + indexDist + " " + middleDist + " " + ringDist + " " + pinkyDist);
        coordText.UpdateTextBox(thumbDist + " \n" + indexDist + " \n" + middleDist + " \n" + ringDist + " \n" + pinkyDist);
        List<float> fingerDistances = new List<float>() { thumbDist, indexDist, middleDist, ringDist, pinkyDist };

        //to track gesture with highest percentage
        float CurrHighestPercentage = minimumTreshold;
        string CurrHighestName = "No gesture detected.";
        //need a list to keep track of all teh gestures
        for (int i = 0; i < gestures.Count; i++)
        {
            float GesturePercentage = CheckGesturePercentage(gestures[i], fingerDistances);
            if (GesturePercentage > CurrHighestPercentage)
            {
                CurrHighestName = gestures[i].gestureName;
                CurrHighestPercentage = GesturePercentage;
            }
            //Debug.Log(gestures[i].gestureName + "Percentage: " + GesturePercentage.ToString());
            percentageText.UpdateTextBox(gestures[i].gestureName + "Percentage: " + GesturePercentage.ToString());
        }

        //This to to update the current gesture being detected
        gestureText.UpdateTextBox(CurrHighestName);
        if (handGestureHandler != null)
            handGestureHandler.currGesture = CurrHighestName;

        return CurrHighestName;
    }
    private float CheckGesturePercentage(Gesture gesture, List<float> fingerDistances)
    {
        float currGesturePercentage = 0;
        float fallOffByPercent = rangeFallOff / 20;
        // check for each finger
        for (int i = 0; i < fingerDistances.Count; i++)
        {
            // check if it is within range of the gesture
            if (gesture.fingerDistances[i].x - tolerance < fingerDistances[i] && gesture.fingerDistances[i].y + tolerance > fingerDistances[i])
            {
                //increase currGesturePrecentage
                currGesturePercentage += 20;
                continue;
            }
            //if (fallOffByPercent != 0)
            //  continue;

            if (gesture.fingerDistances[i].x - tolerance > fingerDistances[i])
            {
                currGesturePercentage += 20 - Mathf.Abs((gesture.fingerDistances[i].x - tolerance - fingerDistances[i]) / fallOffByPercent);
            }
            else if (gesture.fingerDistances[i].y + tolerance < fingerDistances[i])
            {
                currGesturePercentage += 20 - Mathf.Abs((gesture.fingerDistances[i].y - tolerance - fingerDistances[i]) / fallOffByPercent);
            }
        }
        return currGesturePercentage;
    }

}
