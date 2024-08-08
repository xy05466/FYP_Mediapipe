using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LandmarkSO : ScriptableObject
{
    // TODO (Paul): change accupointSOs to use Int + Vec3
    //public Vector4 pointTethering;
    public int landmarkIndex;

    public int GetLandmarkIndex()
    {
        return landmarkIndex;
    }
}
