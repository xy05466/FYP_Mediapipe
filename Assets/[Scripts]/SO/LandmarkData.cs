using UnityEngine;

namespace SDM_FythmPipe
{
    [CreateAssetMenu(menuName = "Scriptable/Landmark Data", fileName = "LandmarkData_")]
    public class LandmarkData : ScriptableObject
    {
        [Range(0,478)]
        public int landmarkIndex;
        public int GetLandmarkIndex() => landmarkIndex;
    }
}