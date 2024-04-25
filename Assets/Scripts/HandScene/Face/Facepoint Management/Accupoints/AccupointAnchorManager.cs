using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AccupointAnchorManager : MonoBehaviour
{
    [SerializeField] FacepointManager_Jaden _facepointManager;
    [SerializeField] AccupointSO accupointSO;
    [SerializeField] GameObject accupointPrefab;
    GameObject parent;
    [HideInInspector] public GameObject accupoint;
    [SerializeField] private Vector3 offset = Vector3.zero;
    private bool isAveraged = true;
    private bool lockedToZero = true;

    private void Start()
    {
        parent = gameObject;
        accupoint = Instantiate(accupointPrefab, parent.transform);
        //accupoint.GetComponent<Accupoint>().SetData(accupointSO);
        accupoint.SetActive(true);
    }
    List<Transform> positions;
    Vector3 Reposition(List<Transform> positions, bool lockToZero, bool isAveraged = true)
    {
        if (positions != null)
        {
            float avgX = 0, avgY = 0, avgZ = 0;
            if (isAveraged)
            {
                foreach (Transform transform in positions)
                {
                    avgX += transform.position.x;
                    avgY += transform.position.y;
                    avgZ += transform.position.z;
                }
                avgX /= positions.Count;
                avgY /= positions.Count;
                avgZ /= positions.Count;
                avgX += offset.x;
                avgY += offset.y;
                avgZ += offset.z;

            }
            else
            {
                avgX = positions.Last().position.x;
                avgY = positions.Last().position.y;
                avgZ = positions.Last().position.z;
            }
            return new Vector3(avgX, avgY, avgZ);
        }
        else return Vector3.zero;
    }

    private void Update()
    {
        if (accupoint.activeInHierarchy)
        {
            positions = _facepointManager.GetLandmarkPositionsByDigit(accupointSO.landmarkIndex);
            accupoint.transform.position = Reposition(positions, lockedToZero, isAveraged);
            if (lockedToZero)
                accupoint.transform.localPosition = new Vector3(accupoint.transform.localPosition.x, accupoint.transform.localPosition.y, 0);
        }
    }

}


