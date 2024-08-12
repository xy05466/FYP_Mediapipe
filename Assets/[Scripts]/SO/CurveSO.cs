using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Curve")]

public class CurveSO : ScriptableObject
{
    public int[] points = new int[4];
}
