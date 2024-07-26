using System;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkManager : MonoBehaviour
{
    public bool isFaceTask = true;
    public bool isHandTask = true;
    public LandmarkListGO annotationList_FacePoints;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints_1;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints_2;

    //public LandmarkListGO annotationList_FacePoints_Task;
    //public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints_Task;


    // variable for Hand Face points active / loaded
    private bool isHandLoadedAndActive = false;
    public bool IsHandLoadedAndActive => isHandLoadedAndActive;
    private bool isHandLoaded = false;
    public bool IsHandLoaded => isHandLoaded;
    private bool isFaceLoadedAndActive = false;
    public bool IsFaceloadedAndActive => isFaceLoadedAndActive;
    private bool isFaceLoaded = false;
    public bool IsFaceLoaded => isFaceLoaded;

    private System.Action<int> OnHandPointsActive;

    public void Initialize()
    {
        annotationList_FacePoints.SubscribePointsSpawn(FacePointsDetected);
        annotationList_HandPoints_1.SubscribePointsSpawn(HandPointsDetected);
        annotationList_HandPoints_2.SubscribePointsSpawn(HandPointsDetected);

        annotationList_HandPoints_1.SubscribeParentActive(HandPointActive);
        annotationList_HandPoints_2.SubscribeParentActive(HandPointActive);

        //annotationList_FacePoints_Task.SubscribePointsSpawn(FacePointsDetected);
        //annotationList_HandPoints_Task.SubscribePointsSpawn(HandPointsDetected);

    }
    int numHandsActive = 0;
    private void HandPointActive(bool active)
    {
        if (active)
            numHandsActive++;
        else
            numHandsActive--;
        OnHandPointsActive?.Invoke(numHandsActive);
    }

    public void UpdateLandmarks()
    {
        //if(isFaceTask)
        //    annotationList_FacePoints_Task.Update();
        //else
            annotationList_FacePoints.Update();

        //if (isHandTask)
        //    annotationList_HandPoints_Task.Update();
        //else
            annotationList_HandPoints_1.Update();

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
        //if (isFaceTask)
        //    return annotationList_FacePoints_Task.GetPoints();
        //else
            return annotationList_FacePoints.GetPoints();
    }
    public List<HandPointAnnotationObject> GetHandPoints(int hand = 0)
    {
        //if(isHandTask)
        //    return annotationList_HandPoints_Task.GetPoints();
        //else
        if(hand == 0)
        {
            return annotationList_HandPoints_1.GetPoints();
        }
        else
        {
            return annotationList_HandPoints_2.GetPoints();
        }
    }

    // Active
    public void SubscribeFacePointsActive(Action<bool> faceActive)
    {
        annotationList_FacePoints.SubscribeParentActive(faceActive);
    }
    public void SubscribeHandPointsActive(Action<int> numHandsActive)
    {
        OnHandPointsActive += numHandsActive;
    }

    // Loaded
    public void SubscribeFacePointsLoaded(Action<List<GameObject>> facepointsLoaded)
    {
        annotationList_FacePoints.SubscribePointsSpawn(facepointsLoaded);
    }
    public void SubscribeHandPointsLoaded_1(Action<List<HandPointAnnotationObject>> handpointsLoaded)
    {
        annotationList_HandPoints_1.SubscribePointsSpawn(handpointsLoaded);
    }
    public void SubscribeHandPointsLoaded_2(Action<List<HandPointAnnotationObject>> handpointsLoaded)
    {
        annotationList_HandPoints_2.SubscribePointsSpawn(handpointsLoaded);
    }
}
