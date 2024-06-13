/// Adopted from homuler's PointListAnnotation.cs file.
/// Written by Paul Baquiran 220150Y
//// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using Mediapipe;
using Mediapipe.Unity;
using System.Collections.Generic;
using UnityEngine;
using mptcc = Mediapipe.Tasks.Components.Containers;
using mplt = Mediapipe.LocationData.Types;
public class FacePointsObjectAnnotation : ListAnnotation<FacePointGO>
{
  private const int _FaceLandmarkCount = 468;
  private const int _IrisLandmarkCount = 5;
  protected override FacePointGO InstantiateChild(bool isActive = true)
  {
    var annotation = base.InstantiateChild(isActive);
    annotation.gameObject.transform.parent = transform;
    return annotation;
  }
  public void Draw(IReadOnlyList<Vector3> targets)
  {
    if (ActivateFor(targets))
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) { annotation.Draw(target); }
      });
    }
  }
  public void Draw(IReadOnlyList<NormalizedLandmarkList> targets, bool visualizeZ = false)
  {
    if (ActivateFor(targets))
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) {
          var (faceLandmarks, leftLandmarks, rightLandmarks) = PartitionLandmarkList(target.Landmark);
          Draw(faceLandmarks, false);
        }
      });
    }
  }

  public void Draw(IReadOnlyList<Landmark> targets, Vector3 scale, bool visualizeZ = true)
  {
    if (ActivateFor(targets))
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) { annotation.Draw(target, scale, visualizeZ); }
      });
    }
  }

  public void Draw(LandmarkList targets, Vector3 scale, bool visualizeZ = true)
  {
    Draw(targets.Landmark, scale, visualizeZ);
  }

  public void Draw(IReadOnlyList<NormalizedLandmark> targets, bool visualizeZ = true)
  {
    if (ActivateFor(targets))
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) { annotation.Draw(target, visualizeZ); }
      });
    }
  }

  public void Draw(NormalizedLandmarkList targets, bool visualizeZ = true)
  {
    Draw(targets.Landmark, visualizeZ);
  }

  public void Draw(IReadOnlyList<mptcc.NormalizedLandmark> targets, bool visualizeZ = true)
  {
    if (ActivateFor(targets))
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) { annotation.Draw(in target, visualizeZ); }
      });
    }
  }

  public void Draw(mptcc.NormalizedLandmarks targets, bool visualizeZ = true) => Draw(targets.landmarks, visualizeZ);

  public void Draw(IReadOnlyList<mplt.RelativeKeypoint> targets, float threshold = 0.0f)
  {
    if (ActivateFor(targets))
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) { annotation.Draw(target, threshold); }
      });
    }
  }

  public void Draw(IReadOnlyList<mptcc.NormalizedKeypoint> targets, float threshold = 0.0f)
  {
    if (ActivateFor(targets))
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) { annotation.Draw(target, threshold); }
      });
    }
  }
  private static (IReadOnlyList<T>, IReadOnlyList<T>, IReadOnlyList<T>) PartitionLandmarkList<T>(IReadOnlyList<T> landmarks)
  {
    if (landmarks == null)
    {
      return (null, null, null);
    }

    var enumerator = landmarks.GetEnumerator();
    var faceLandmarks = new List<T>(_FaceLandmarkCount);
    for (var i = 0; i < _FaceLandmarkCount; i++)
    {
      if (enumerator.MoveNext())
      {
        faceLandmarks.Add(enumerator.Current);
      }
    }
    if (faceLandmarks.Count < _FaceLandmarkCount)
    {
      return (null, null, null);
    }

    var leftIrisLandmarks = new List<T>(_IrisLandmarkCount);
    for (var i = 0; i < _IrisLandmarkCount; i++)
    {
      if (enumerator.MoveNext())
      {
        leftIrisLandmarks.Add(enumerator.Current);
      }
    }
    if (leftIrisLandmarks.Count < _IrisLandmarkCount)
    {
      return (faceLandmarks, null, null);
    }

    var rightIrisLandmarks = new List<T>(_IrisLandmarkCount);
    for (var i = 0; i < _IrisLandmarkCount; i++)
    {
      if (enumerator.MoveNext())
      {
        rightIrisLandmarks.Add(enumerator.Current);
      }
    }
    return rightIrisLandmarks.Count < _IrisLandmarkCount ? (faceLandmarks, leftIrisLandmarks, null) : (faceLandmarks, leftIrisLandmarks, rightIrisLandmarks);
  }

}
