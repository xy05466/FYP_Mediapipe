using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PageLogic_Extension gives extra functions to monitor face or hand landmarks are active or inactive
public abstract class PageLogic_Extension : PageLogic
{
    public abstract override void EndPage();

    public abstract override bool IsPageDone();

    public abstract override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject);

    public abstract override void ResetPage();

    public abstract override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj);

    public abstract override void StartPage();

    // Extra functions

    public abstract void OnFaceAnnotationActive(bool active);

    public abstract void OnHandAnnotationActive(int actives);

  
}
