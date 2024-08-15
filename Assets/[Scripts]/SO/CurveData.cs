using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
    [CreateAssetMenu(menuName = "Scriptable/Curve Data", fileName = "CurveData_")]

    public class CurveData : ScriptableObject
    {
        public int[] points = new int[4];
    }
}

