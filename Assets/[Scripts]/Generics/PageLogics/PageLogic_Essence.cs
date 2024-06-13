using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageLogic_Essence : PageLogic_Extension
{
    public FacepointManager_Jaden facepointManager_Jaden;
    public AccupointsMassageManagerFinal accupointsMassageManagerFinal;
    public override void StartPage()
    {
    }
 
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
    }
    public override void EndPage()
    {
    }
    public override void ResetPage()
    {
    }

    public override bool IsPageDone()
    {
        return accupointsMassageManagerFinal.IsDone();
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



 
}
