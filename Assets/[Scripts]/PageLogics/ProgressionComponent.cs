using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProgressionComponent : MonoBehaviour
{
    public abstract void UpdateProgression(float subProgression,int currentProgress, int maxProgress);
    public abstract void OnProgression(int currentProgression, int maxProgression);
}
