using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageLogic_Essence : PageLogic_Extension
{
    public FacepointManager_Jaden facepointManager_Jaden;
    public AccupointsMassageManagerFinal accupointsMassageManagerFinal;
    public override void StartPage()
    {
        accupointsMassageManagerFinal?.StartFunc();
    }
 
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if(accupointsMassageManagerFinal.gameObject.activeInHierarchy)
            accupointsMassageManagerFinal.UpdateScene();
    }
    public override void EndPage()
    {
    }
    public override void ResetPage()
    {
    }

    public override bool IsPageDone()
    {
        //return accupointsMassageManagerFinal.IsDone();
        return false;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
        accupointsMassageManagerFinal.gameObject.SetActive(active);
    }

    public override void OnHandAnnotationActive(bool active)
    {
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
    }



 
}
