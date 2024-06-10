using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointIndex : MonoBehaviour
{
    private int pointIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        pointIndex = transform.GetSiblingIndex();
    }
    public int GetIndex()
    {
        return pointIndex;
    }
    public void InitIndex()
    {
        pointIndex = transform.GetSiblingIndex();
    }
}
