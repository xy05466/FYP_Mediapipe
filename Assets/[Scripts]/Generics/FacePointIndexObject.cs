using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
