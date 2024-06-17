using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_PhotoCapture : PageLogic_Extension
{
    public Button bu_done;
    public GameObject photoOverlay;
    public PhoneCamera phoneCamera;
    bool done = false;
    public override void StartPage()
    {
        bu_done.onClick.AddListener(()=> { done = true; });
        phoneCamera.StartPhoto();
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
    }


    public override void EndPage()
    {
        bu_done.onClick.RemoveAllListeners();
        photoOverlay.gameObject.SetActive(false);
        done = false;
    }

    public override bool IsPageDone()
    {
        return done;
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

   
}
