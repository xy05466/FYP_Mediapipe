using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageLogic_Cream : PageLogic_Extension
{
    public CreamSceneManager creamManager;
    public override void StartPage()
    {
    }

    public override void ResetPage()
    {
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (facepointsObj == null || facepointsObj.Count <=0 || creamManager.allPetals ==null || creamManager.allPetals.Count <=0) return;
        for (int i = 0; i < creamManager.allPetals.Count; i++)
        {
            creamManager.allPetals[i].transform.position = facepointsObj[creamManager.allPetals[i].pointId].transform.position;
        }
    }

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
        Debug.Log("collide");
        foreach (var petal in creamManager.allPetals)
        {
            if (petal.pointId == facePointIndex)
            {
                petal.Fill();
            }
        }
    }

   
}
