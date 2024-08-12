using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable/Landmark Data", fileName = "LandmarkData_")]
public class LandmarkData : ScriptableObject
{
    public int landmarkIndex;

    public int GetLandmarkIndex()
    {
        return landmarkIndex;
    }
}
