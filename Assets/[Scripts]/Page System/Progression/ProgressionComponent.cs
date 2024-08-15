using UnityEngine;

namespace SDM_FythmPipe
{
    public abstract class ProgressionComponent : MonoBehaviour
    {
        public abstract void UpdateProgression(float subProgression, int currentProgress, int maxProgress);
        //public abstract void OnProgressed(int currentProgression, int maxProgression);
    }
}

