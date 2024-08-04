using System.Collections.Generic;
using UnityEngine;

public class PageLogic_Cream : PageLogic_Extension
{
    [SerializeField] private List<GameObject> Wallpapers = new List<GameObject>();
    [SerializeField] private GameObject imageSegOBJ;
    private ImageSeg imageSeg;
    public CreamSceneManager creamManager;
    int petalCount = 0;
    int petalDone = 0;
    bool done = false;
    public override void Initialize()
    {
        creamManager.Init();
        petalCount = creamManager.allPetals.Count;
        foreach (FlowerAccupoint fc in creamManager.allPetals) 
        {
            fc.Subscribe(OnPetalDone);
        }
    }
    private void OnPetalDone(FlowerAccupoint accupoint)
    {
        petalDone ++;
        if(petalDone>= petalCount)
        {
            done = true;
        }
    }
    public override void StartPage()
    {
        done = false;
        petalDone = 0;
        foreach (FlowerAccupoint fc in creamManager.allPetals)
            fc.ResetPetal();

            if (imageSegOBJ)
            imageSeg = imageSegOBJ.GetComponent<ImageSeg>();
    }

    public override void ResetPage()
    {
    }

    public override void EndPage()
    {
    }

    public override bool IsPageDone()
    {
        return done;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
    }

    public override void OnHandAnnotationActive(int actives)
    {
    }

    public override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject)
    {
        if (!gameObject.activeInHierarchy) return;
        foreach (var petal in creamManager.allPetals)
        {
            if (petal.pointId == facePointIndex)
            {
                petal.Fill();
            }
        }
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
    {
        if (!gameObject.activeInHierarchy) return;
        if (facepointsObj==null || facepointsObj.Count<=0) return;
        foreach (var petal in creamManager.allPetals)
        {
            petal.transform.position = facepointsObj[petal.pointId].transform.position;
        }

        //if (handpointsObj_1.Count > 0)
        //{
        //    HandPointAnnotationObject m = handpointsObj_1[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.MIDDLE_FINGER_TIP)];
        //    HandPointAnnotationObject i = handpointsObj_1[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.INDEX_FINGER_TIP)];
        //    HandPointAnnotationObject r = handpointsObj_1[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.RING_FINGER_TIP)];
        //    HandPointAnnotationObject p = handpointsObj_1[FaceHandPoint.GetHandPointIndex(FaceHandPoint.HANDPOINT.PINKY_TIP)];

        //    for (int j = 0; j < Wallpapers.Count; j++)
        //    {
        //        BoxCollider2D wallpaper = Wallpapers[j].GetComponent<BoxCollider2D>();
        //        SphereCollider fingerPoints1 = m.GetComponent<SphereCollider>();
        //        SphereCollider fingerPoints2 = i.GetComponent<SphereCollider>();
        //        SphereCollider fingerPoints3 = r.GetComponent<SphereCollider>();
        //        SphereCollider fingerPoints4 = p.GetComponent<SphereCollider>();
        //        List<SphereCollider>contactPoints = new List<SphereCollider>();

        //        contactPoints.Add(fingerPoints1);
        //        contactPoints.Add(fingerPoints2);
        //        contactPoints.Add(fingerPoints3);
        //        contactPoints.Add(fingerPoints4);

        //        if (imageSeg)
        //        {
        //            foreach (var contactpoint in contactPoints)
        //        {
        //            switch (j)
        //            {
        //                case 0:
        //                    if (IsColliding(contactpoint, wallpaper))
        //                    {
        //                        imageSeg.PlayWallpaper(j + 1);
        //                    }
        //                    break;
        //                case 1:
        //                    if (IsColliding(contactpoint, wallpaper))
        //                    {
        //                        imageSeg.PlayWallpaper(j + 1);
        //                    }
        //                    break;
        //                case 2:
        //                    if (IsColliding(contactpoint, wallpaper))
        //                    {
        //                        imageSeg.PlayWallpaper(j + 1);
        //                    }
        //                    break;
        //                case 3:
        //                    if (IsColliding(contactpoint, wallpaper))
        //                    {
        //                        imageSeg.PlayWallpaper(j + 1);
        //                    }
        //                    break;
        //                default:
        //                    Debug.LogError("Invalid wallpaper index.");
        //                    break;
        //            }
        //        }   
        //        }
        //    }
        //}
    }

    bool IsColliding(SphereCollider threeD, BoxCollider2D ui)
    {
        // Convert the 3D collider bounds to screen space
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(threeD.bounds.center);

        // Check if the screen point is within the bounds of the 2D collider
        return ui.OverlapPoint(screenPoint);
    }

  
}
