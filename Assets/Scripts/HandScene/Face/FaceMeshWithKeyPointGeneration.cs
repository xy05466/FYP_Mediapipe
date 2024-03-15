using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Sample.FaceMesh
{
  public class FaceMeshWithKeyPointGeneration : ImageSourceSolution<FaceMeshGraph>
  {
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private MultiFaceLandmarkListAnnotationController _multiFaceLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromDetectionsAnnotationController;
    [SerializeField] private FaceLandmarkTranslator _faceLandmarkTranslator;

    public int maxNumFaces
    {
      get => graphRunner.maxNumFaces;
      set => graphRunner.maxNumFaces = value;
    }

    public bool refineLandmarks
    {
      get => graphRunner.refineLandmarks;
      set => graphRunner.refineLandmarks = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
      get => graphRunner.minTrackingConfidence;
      set => graphRunner.minTrackingConfidence = value;
    }

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnFaceDetectionsOutput += OnFaceDetectionsOutput;
        graphRunner.OnMultiFaceLandmarksOutput += OnMultiFaceLandmarksOutput;
        graphRunner.OnFaceRectsFromLandmarksOutput += OnFaceRectsFromLandmarksOutput;
        graphRunner.OnFaceRectsFromDetectionsOutput += OnFaceRectsFromDetectionsOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_faceDetectionsAnnotationController, imageSource);
      SetupAnnotationController(_faceRectsFromLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_multiFaceLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_faceRectsFromDetectionsAnnotationController, imageSource);
      SetupAnnotationController(_faceLandmarkTranslator, imageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      var task = graphRunner.WaitNext();
      yield return new WaitUntil(() => task.IsCompleted);

      var result = task.Result;
      //_faceDetectionsAnnotationController.DrawNow(result.faceDetections);
      _multiFaceLandmarksAnnotationController.DrawNow(result.multiFaceLandmarks);
      //_faceRectsFromLandmarksAnnotationController.DrawNow(result.faceRectsFromLandmarks);
      //_faceRectsFromDetectionsAnnotationController.DrawNow(result.faceRectsFromDetections);
      //_faceLandmarkTranslator.DrawNow(result.multiFaceLandmarks);
    }

    private void OnFaceDetectionsOutput(object stream, OutputStream<List<Detection>>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(Detection.Parser);
      _faceDetectionsAnnotationController.DrawLater(value);
    }

    private void OnMultiFaceLandmarksOutput(object stream, OutputStream<List<NormalizedLandmarkList>>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
      _multiFaceLandmarksAnnotationController.DrawLater(value);
      //_faceLandmarkTranslator.DrawLater(value);
    }

    private void OnFaceRectsFromLandmarksOutput(object stream, OutputStream<List<NormalizedRect>>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedRect.Parser);
      //_faceRectsFromLandmarksAnnotationController.DrawLater(value);
    }

    private void OnFaceRectsFromDetectionsOutput(object stream, OutputStream<List<NormalizedRect>>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedRect.Parser);
      //_faceRectsFromDetectionsAnnotationController.DrawLater(value);
    }
  }
}
