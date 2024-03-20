using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accupoint : MonoBehaviour
{
    [SerializeField] AccupointSO accupointData;
    public void SetData(AccupointSO data)
    {
        accupointData = data;
    }
    void RepositionPoint(Vector3 newPoint)
    {
        gameObject.transform.position = newPoint;
    }
    public AccupointSO GetData()
    {
        return accupointData;
    }
}
