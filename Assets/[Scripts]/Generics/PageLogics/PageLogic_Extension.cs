using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageLogic_Extension : PageLogic
{
    public override void EndPage()
    {
    }

    public override bool IsPageDone()
    {
        return false;
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
    }

    public override void ResetPage()
    {
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
    }

    public override void StartPage()
    {
    }

    public abstract void OnFaceAnnotationActive(bool active);

    public abstract void OnHandAnnotationActive(bool active);
}
