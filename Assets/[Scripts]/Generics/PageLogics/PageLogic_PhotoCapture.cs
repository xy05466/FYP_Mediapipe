using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_PhotoCapture : PageLogic_Extension
{
    public Button bu_done;
    bool done = false;
    public override void StartPage()
    {
        bu_done.onClick.AddListener(()=> { done = true; });
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
    }


    public override void EndPage()
    {
        bu_done.onClick.RemoveAllListeners();
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
        throw new System.NotImplementedException();
    }

   
}
