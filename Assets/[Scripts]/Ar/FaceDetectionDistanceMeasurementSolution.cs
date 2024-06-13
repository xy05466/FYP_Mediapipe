/// Adopted from homuler's FaceDetectionSolution.cs file
/// Written by Paul Baquiran 220150Y / JustACoderWonderingAround on GitHub
// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Sample.FaceDetection
{
  public class FaceDetectionDistanceMeasurementSolution : ImageSourceSolution<FaceDetectionGraph>
  {
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private FaceDistanceController _faceDistanceController;

    public FaceDetectionGraph.ModelType modelType
    {
      get => graphRunner.modelType;
      set => graphRunner.modelType = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnFaceDetectionsOutput += OnFaceDetectionsOutput;
      }

      SetupAnnotationController(_faceDetectionsAnnotationController, ImageSourceProvider.ImageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      var task = graphRunner.WaitNext();
      yield return new WaitUntil(() => task.IsCompleted);

      _faceDetectionsAnnotationController.DrawNow(task.Result);
    }

    private void OnFaceDetectionsOutput(object stream, OutputStream<List<Detection>>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(Detection.Parser);
      _faceDetectionsAnnotationController.DrawLater(value);
      //TranslateFaceDetection(value);
      _faceDistanceController.PrintFaceGestures(value);
    }
    private void TranslateFaceDetection(List<Detection> value)
    {
      int counter = 0;
      foreach (Detection detection in value)
      {
        counter++;
        Debug.Log("Detection " + counter + ": " + detection.DisplayName);
      }
    }
  }
}
