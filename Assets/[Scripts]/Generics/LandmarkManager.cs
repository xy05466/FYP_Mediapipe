using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkManager : MonoBehaviour
{
    public LandmarkListGO annotationList_FacePoints;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints;
    private bool isHandLoadedAndActive = false;
    private bool isHandLoaded = false;
    public bool IsHandLoadedAndActive => isHandLoadedAndActive;
    public bool IsHandLoaded => isHandLoaded;
    private bool isFaceLoadedAndActive = false;
    private bool isFaceLoaded = false;
    public bool IsFaceloadedAndActive => isFaceLoadedAndActive;
    public bool IsFaceLoaded => isFaceLoaded;

    private void Start()
    {
        annotationList_FacePoints.SubscribePointsSpawn(FacePointsDetected);
        annotationList_HandPoints.SubscribePointsSpawn(HandPointsDetected);
    }

    private void Update()
    {
        annotationList_FacePoints.Update();
        annotationList_HandPoints.Update();
    }
    // Store all points
    private void FacePointsDetected(List<GameObject> facePoints)
    {
        isFaceLoadedAndActive = true;
        if (!isFaceLoaded)
            isFaceLoaded = true;

    }
    private void HandPointsDetected(List<HandPointAnnotationObject> handPoints)
    {
        isHandLoadedAndActive = true;
        if (!isHandLoaded)
            isHandLoaded = true;
    }

    public List<GameObject> GetFacePoints()
    {
        return annotationList_FacePoints.GetPoints();
    }
    public List<HandPointAnnotationObject> GetHandPoints()
    {
        return annotationList_HandPoints.GetPoints();
    }

    public void SubscribePointsActive(Action<bool> facepointsActive, Action<bool> handpointsActive)
    {
        annotationList_FacePoints.SubscribeParentActive(facepointsActive);
        annotationList_HandPoints.SubscribeParentActive(handpointsActive);
    }

    // Check loaded and active
}
