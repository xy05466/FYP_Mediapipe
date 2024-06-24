using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_Essence : PageLogic_Extension
{
    public FacepointManager_Jaden facepointManager_Jaden;
    public AccupointsMassageManagerFinal accupointsMassageManagerFinal;
    [Header("TESTING point")]
    public List<int> points = new List<int>();
    public LineRenderer lr;
    public LineRenderer lr2;
    public Texture testSprite;
    public int positionCount =1; // should start at 1
    public Button bu_fill;
    Vector3 lerpPos = Vector3.zero;
    //lr.GetPosition(i-1) / 
    public override void StartPage()
    {
        //accupointsMassageManagerFinal?.StartFunc();
        bu_fill.onClick.AddListener(Fill);
        //lr.materials[0].SetTexture("_MainTex", testSprite);
        lr.positionCount = points.Count;
        lr2.positionCount = positionCount;
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (facepointsObj == null  || facepointsObj.Count <=0) return;
        //if(accupointsMassageManagerFinal.gameObject.activeInHierarchy)
        //    accupointsMassageManagerFinal.UpdateScene();

        for(int i = 0; i < points.Count; i++)
        {
            int p = points[i];
            lr.SetPosition(i, facepointsObj[p].transform.position);
        }
        for (int i = 0;i < positionCount; i++)
        {
            //if(i == positionCount - 1)
            //{
            //    lerpPos = Vector3.MoveTowards(lerpPos,lr.GetPosition(i),Time.deltaTime);
            //    //lerp current pos to next
            //    lr2.SetPosition(i, lerpPos);
            //}
            //else
            //{
            //    lr2.SetPosition(i, lr.GetPosition(i));
            //}
            lr2.SetPosition(i, lr.GetPosition(i));

        }

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
        //accupointsMassageManagerFinal.gameObject.SetActive(active);
    }

    public override void OnHandAnnotationActive(bool active)
    {
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
    }

    public void Fill()
    {
        positionCount++;
        Debug.Log(positionCount);
        if (positionCount > points.Count)
        {
            positionCount = points.Count;
        }
        lr2.positionCount = positionCount;
        lr2.SetPosition(positionCount - 1,lr2.GetPosition(positionCount-2));
        lerpPos = lr2.GetPosition(positionCount - 2);
    }


}
