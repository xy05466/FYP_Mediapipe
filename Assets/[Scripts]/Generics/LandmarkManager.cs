using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkManager : MonoBehaviour
{
    public LandmarkListGO annotationList_FacePoints;
    public LandmarkList<HandPointAnnotationObject> annotationList_HandPoints;
    private bool isHandLoadedAndActive = false;
    public bool IsHandLoadedAndActive => isHandLoadedAndActive;
    private bool isFaceloadedAndActive = false;
    public bool IsFaceloadedAndActive => isFaceloadedAndActive;

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
        //this.facePoints = facePoints;
        //foreach (var pageL in pageLogics)
        //    pageL.LoadFacePoints(true);
        isFaceloadedAndActive = true;

    }
    private void HandPointsDetected(List<HandPointAnnotationObject> handPoints)
    {
        isHandLoadedAndActive = true;

        //this.handPoints = handPoints;
        //foreach (var item in handPoints)
        //    item.OnCollidingFacePoint += OnHandPointCollide;

        //handPointsStored = true;

        //if (!currentPageLogic.HandpointsLoaded)
        //{
        //    foreach (var item in handPoints)
        //        item.OnCollidingFacePoint += currentPageLogic.OnHandPointCollideFacePoint;
        //    currentPageLogic.LoadHandPoints(true);
        //}
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
