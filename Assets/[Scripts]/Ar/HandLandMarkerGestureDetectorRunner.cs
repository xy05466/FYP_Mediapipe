/// Adopted from homuler's Mediapipe for Unity Plugin's HandLandmarkerRunner.cs file.
/// Modifications made by Paul Baquiran / JustACoderWanderingAround on github / Admin num 220150Y
/// Refactored by Raqib

// Copyright (c) 2023 homuler
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe;


public class HandLandMarkerGestureDetectorRunner  : HandLandmarkerRunner
{
    //[SerializeField] private HandLandmarkModelTranslator _handLandmarkModelTranslator;
    [SerializeField] private HandGestureManager handGestureManager;

    protected override void OnHandLandmarkDetectionOutput(HandLandmarkerResult result, Image image, long timestamp)
    {
        base.OnHandLandmarkDetectionOutput(result, image, timestamp);
        if (result.handLandmarks != null)
        {
            //var gestureResult = handGestureManager.ReadGesture(result);
        }
        else
        {
            //handGestureManager.NoGesture();
        }
    }
}

