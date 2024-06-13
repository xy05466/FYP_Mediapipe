using System.Collections.Generic;
using UnityEngine;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Tasks.Vision.FaceLandmarker;

public class FaceLandmarkTranslator : AnnotationController<FacePointsObjectAnnotation>
{
  [SerializeField] FacePointGOPool pointsPool;

  private IReadOnlyList<NormalizedLandmarkList> _currentTarget;
  private FaceLandmarkerResult _resultTarget;

  public void DrawNow(IReadOnlyList<NormalizedLandmarkList> target)
  {
    _currentTarget = target;
    SyncNow();
  }
  public void DrawLater(IReadOnlyList<NormalizedLandmarkList> target)
  {
    UpdateCurrentTarget(target, ref _currentTarget);
  }
  protected override void SyncNow()
  {
    isStale = false;
    annotation.Draw(_currentTarget, false);
  }
}
