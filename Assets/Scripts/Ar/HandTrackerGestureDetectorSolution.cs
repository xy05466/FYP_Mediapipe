/// Adopted from homuler's Mediapipe for Unity Plugin's HandTrackingSolution.cs file.
/// Modifications made by Paul Baquiran / JustACoderWanderingAround on github / Admin num 220150Y
// Copyright (c) 2023 homuler
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.Sample.HandTracking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandTrackerGestureDetectorSolution : HandTrackingSolution
{
    //private void OnHandGestureOutput(object stream, OutputStream<List<ClassificationList>>.OutputEventArgs eventArgs)
    //{
    //  var packet = eventArgs.packet;
    //  var value = packet == null ? default : packet.Get(ClassificationList.Parser);
    //  _handLandmarksAnnotationController.DrawLater(value);
    //}
}
