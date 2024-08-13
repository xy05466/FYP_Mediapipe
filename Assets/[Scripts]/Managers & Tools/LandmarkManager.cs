using System;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkManager : MonoBehaviour
{
    public LandmarkListGO annotationList_FacePoints;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints_1;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints_2;


    // variable for Hand Face points active / loaded
    private bool isHandLoadedAndActive_1 = false;
    public bool IsHandLoadedAndActive_1 => isHandLoadedAndActive_1;
    private bool isHandLoadedAndActive_2 = false;
    public bool IsHandLoadedAndActive_2 => isHandLoadedAndActive_2;
    //private bool isHandLoaded = false;
    private int numHandsLoaded = 0;
    public int NumHandsLoaded => numHandsLoaded;
    private int numHandsActive = 0;
    public int NumHandsActive => numHandsActive;

    //public bool IsHandLoaded => isHandLoaded;

    private bool isFaceLoadedAndActive = false;
    public bool IsFaceloadedAndActive => isFaceLoadedAndActive;
    private bool isFaceLoaded = false;
    public bool IsFaceLoaded => isFaceLoaded;

    private System.Action<int> OnHandPointsActive;

    private void Start()
    {
        annotationList_HandPoints_2.SetStartIndex(22);
        annotationList_FacePoints.SubscribePointsSpawn(FacePointsDetected);
        annotationList_HandPoints_1.SubscribePointsSpawn(HandPointsDetected);
        annotationList_HandPoints_2.SubscribePointsSpawn(HandPointsDetected);

        annotationList_HandPoints_1.SubscribeParentActive(HandPointActive);
        annotationList_HandPoints_2.SubscribeParentActive(HandPointActive);
        //annotationList_FacePoints_Task.SubscribePointsSpawn(FacePointsDetected);
        //annotationList_HandPoints_Task.SubscribePointsSpawn(HandPointsDetected);

    }
    private void Update()
    {
        annotationList_FacePoints.Update();

        annotationList_HandPoints_1.Update();
        annotationList_HandPoints_2.Update();
    }

    private void HandPointActive(bool active)
    {
        if (active)
            numHandsActive++;
        else
            numHandsActive--;
        OnHandPointsActive?.Invoke(numHandsActive);
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
        if(numHandsActive==0)
            isHandLoadedAndActive_1 = true;
        else
            isHandLoadedAndActive_2 = true;
        //if (!isHandLoaded)
        //    isHandLoaded = true;

        numHandsLoaded++;

    }


    public List<GameObject> GetFacePoints()
    {
        //if (isFaceTask)
        //    return annotationList_FacePoints_Task.GetPoints();
        //else
            return annotationList_FacePoints.GetPoints();
    }

    // Get hand points on right / left hand
    public List<HandPointAnnotationObject> GetHandPoints(int hand)
    {
        //if(isHandTask)
        //    return annotationList_HandPoints_Task.GetPoints();
        //else
        if(hand == 0)
            return annotationList_HandPoints_1.GetPoints();
        else
            return annotationList_HandPoints_2.GetPoints();
    }

    // EVENTS
    // Active
    public void SubscribeFacePointsActive(Action<bool> faceActive)
    {
        annotationList_FacePoints.SubscribeParentActive(faceActive);
    }
    public void SubscribeHandPointsActive(Action<int> handActive)
    {
        OnHandPointsActive += handActive;
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
