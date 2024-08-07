using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageLogic_Extension : PageLogic
{
    public abstract override void EndPage();

    public abstract override bool IsPageDone();

    public abstract override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject);

    public abstract override void ResetPage();

    public abstract override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj);

    public abstract override void StartPage();

    public abstract void OnFaceAnnotationActive(bool active);

    public abstract void OnHandAnnotationActive(bool active);
}
