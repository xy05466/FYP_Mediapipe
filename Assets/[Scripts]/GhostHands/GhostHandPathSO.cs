using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GhostHandPathSO : ScriptableObject
{
    public float maxPathTime;
    public GameObject pathObject;
    public List<int> pathWaypoints;
}
