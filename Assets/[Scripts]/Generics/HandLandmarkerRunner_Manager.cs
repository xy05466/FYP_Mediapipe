using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;

public class HandLandmarkerRunner_Manager : HandLandmarkerRunner
{
    protected override void OnHandLandmarkDetectionOutput(HandLandmarkerResult result, Image image, long timestamp)
    {
        base.OnHandLandmarkDetectionOutput(result, image, timestamp);
        //Debug.Log("landmark get");
    }
}
