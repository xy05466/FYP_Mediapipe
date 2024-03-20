/// Adopted from homuler's PointListAnnotation.cs file.
/// Written by Paul Baquiran 220150Y
// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

using mplt = Mediapipe.LocationData.Types;
using mptcc = Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity;
using Mediapipe;
using Color = UnityEngine.Color;

public class FacePointGO : HierarchicalAnnotation
{
    [HideInInspector]
    public FacePointGOPool objectPool;
    public bool collided;

    #region homuler's code
    public void Draw(Vector3 position)
    {
    SetActive(true); // Vector3 is not nullable
    transform.localPosition = position;
    }

    public void Draw(Landmark target, Vector3 scale, bool visualizeZ = true)
    {
    if (ActivateFor(target))
    {
        var position = GetScreenRect().GetPoint(target, scale, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
        position.z = 0.0f;
        }
        transform.localPosition = position;
    }
    }

    public void Draw(NormalizedLandmark target, bool visualizeZ = true)
    {
    if (ActivateFor(target))
    {
        var position = GetScreenRect().GetPoint(target, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
        position.z = 0.0f;
        }
        transform.localPosition = position;
    }
    }

    public void Draw(in mptcc.NormalizedLandmark target, bool visualizeZ = true)
    {
    if (ActivateFor(target))
    {
        var position = GetScreenRect().GetPoint(in target, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
        position.z = 0.0f;
        }
        transform.localPosition = position;
    }
    }

    public void Draw(mplt.RelativeKeypoint target, float threshold = 0.0f)
    {
    if (ActivateFor(target))
    {
        Draw(GetScreenRect().GetPoint(target, rotationAngle, isMirrored));
    }
    }

    public void Draw(mptcc.NormalizedKeypoint target, float threshold = 0.0f)
    {
    if (ActivateFor(target))
    {
        Draw(GetScreenRect().GetPoint(target, rotationAngle, isMirrored));
    }
    }
    #endregion

    #region Paul's code
    private void OnCollisionStay(Collision collision)
    {
        GetComponent<Renderer>().material.color = Color.red;
        collided = true;
    }
    private void OnCollisionExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
        collided = false;
    }
    #endregion
}
