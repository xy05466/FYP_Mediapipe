using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHandsManager : MonoBehaviour
{
    [SerializeField] FacepointManager fpm;
    [SerializeField] HandPointManager hpm;
    [SerializeField] GameObject ghosthandPrefab;

    GameObject[] handpoints;
    GameObject[] facepoints;

    private void Awake()
    {
        
    }
    void Update()
    {
        handpoints = hpm.GetFacePointTransforms();
        facepoints = fpm.GetFacePointTransforms();
        if (handpoints != null)
        {

        }
        if (facepoints != null)
        {

        }
    }
}
