using UnityEngine;

namespace SDM_FythmPipe
{
    [CreateAssetMenu(menuName = "Scriptable/Landmark Data", fileName = "LandmarkData_")]
    public class LandmarkData : ScriptableObject
    {
        public int landmarkIndex;
        public int GetLandmarkIndex() => landmarkIndex;
    }
}