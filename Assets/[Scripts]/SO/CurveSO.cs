using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Curve Data", fileName ="CurveData_")]

public class CurveSO : ScriptableObject
{
    public int[] points = new int[4];
}
