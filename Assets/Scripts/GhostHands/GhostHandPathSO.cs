using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GhostHandPathSO : ScriptableObject
{
    public GameObject pathObject;
    public List<int> pathWaypoints;
}
