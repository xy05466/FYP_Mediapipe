using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkManager : MonoBehaviour
{
    public bool isFaceTask = true;
    public bool isHandTask = true;
    public LandmarkListGO annotationList_FacePoints;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints;

    public LandmarkListGO annotationList_FacePoints_Task;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints_Task;


    // variable for Hand Face points active / loaded
    private bool isHandLoadedAndActive = false;
    public bool IsHandLoadedAndActive => isHandLoadedAndActive;
    private bool isHandLoaded = false;
    public bool IsHandLoaded => isHandLoaded;
    private bool isFaceLoadedAndActive = false;
    public bool IsFaceloadedAndActive => isFaceLoadedAndActive;
    private bool isFaceLoaded = false;
    public bool IsFaceLoaded => isFaceLoaded;

    public void Initialize()
    {
        annotationList_FacePoints.SubscribePointsSpawn(FacePointsDetected);
        annotationList_HandPoints.SubscribePointsSpawn(HandPointsDetected);

        annotationList_FacePoints_Task.SubscribePointsSpawn(FacePointsDetected);
        annotationList_HandPoints_Task.SubscribePointsSpawn(HandPointsDetected);
    }

    public void UpdateLandmarks()
    {
            annotationList_FacePoints_Task.Update();
            annotationList_FacePoints.Update();

            annotationList_HandPoints_Task.Update();
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
        if (isFaceTask)
            return annotationList_FacePoints_Task.GetPoints();
        else
            return annotationList_FacePoints.GetPoints();
    }
    public List<HandPointAnnotationObject> GetHandPoints()
    {
        if(isHandTask)
            return annotationList_HandPoints_Task.GetPoints();
        else
            return annotationList_HandPoints.GetPoints();
    }

    public void SubscribePointsActive(Action<bool> facepointsActive, Action<bool> handpointsActive)
    {
        annotationList_FacePoints.SubscribeParentActive(facepointsActive);
        annotationList_HandPoints.SubscribeParentActive(handpointsActive);

        annotationList_FacePoints_Task.SubscribeParentActive(facepointsActive);
        annotationList_HandPoints_Task.SubscribeParentActive(handpointsActive);
    }

    public void SubscribePointsLoaded(Action<List<GameObject>> facepointsLoaded, Action<List<HandPointAnnotationObject>> handpointsLoaded)
    {
        annotationList_FacePoints.SubscribePointsSpawn(facepointsLoaded);
        annotationList_HandPoints.SubscribePointsSpawn(handpointsLoaded);

        annotationList_FacePoints_Task.SubscribePointsSpawn(facepointsLoaded);
        annotationList_HandPoints_Task.SubscribePointsSpawn(handpointsLoaded);
    }
}
