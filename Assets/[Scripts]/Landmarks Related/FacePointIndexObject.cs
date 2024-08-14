using UnityEngine;

namespace SDM_FythmPipe
{
    public class FacePointIndexObject : MonoBehaviour
    {
        private int facePointIndex = -1;
        public int GetIndex()
        {
            return facePointIndex;
        }
        public void SetIndex(int index)
        {
            facePointIndex = index;
        }
    }
}