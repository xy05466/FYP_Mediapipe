using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_ImageSeg : PageLogic_Extension
{
    [SerializeField] private List<GameObject> Wallpapers = new List<GameObject>();
    private ImageSeg imageSeg;
    public override void EndPage()
    {
    }

    public override bool IsPageDone()
    {
        return false;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
    }

    public override void OnHandAnnotationActive(bool active)
    {
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
    }

    public override void ResetPage()
    {
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        HandPointAnnotationObject m = handpointsObj[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.MIDDLE_FINGER_TIP)];
        HandPointAnnotationObject i = handpointsObj[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.INDEX_FINGER_TIP)];
        HandPointAnnotationObject r = handpointsObj[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.RING_FINGER_TIP)];
        HandPointAnnotationObject p = handpointsObj[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.PINKY_TIP)];

        for (int j = 0; j < Wallpapers.Count; j++)
        {
            GameObject wallpaper = Wallpapers[j];
            if (IsColliding(m, wallpaper) || IsColliding(i, wallpaper) || IsColliding(r, wallpaper) || IsColliding(p, wallpaper))
            {
                Debug.Log("asdasdasd");
                //imageSeg.PlayWallpaper(j); // Assuming wallpapers are indexed starting from 1
            }
        }
    }

    public override void StartPage()
    {
        imageSeg = GetComponent<ImageSeg>();
    }

    private bool IsColliding(HandPointAnnotationObject handPoint, GameObject obj)
    {
        if (handPoint == null || obj == null)
        {
            return false;
        }

        // Get hand point position
        Vector3 handPointPosition = handPoint.transform.position;

        // Get the collider of the object
        Collider objCollider = obj.GetComponent<Collider>();

        if (objCollider != null)
        {
            // Check if the hand point position is inside the collider bounds
            return objCollider.bounds.Contains(handPointPosition);
        }

        return false;
    }
}
