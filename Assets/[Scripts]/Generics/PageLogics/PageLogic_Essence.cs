using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class PageLogic_Essence : PageLogic_Extension
{
    public FacepointManager_Jaden facepointManager_Jaden;
    public AccupointsMassageManagerFinal accupointsMassageManagerFinal;
    [Header("TESTING point")]
    public List<int> points = new List<int>();
    public int segments = 6;
    public LineRenderer lr;
    public LineRenderer lr2;
    public Texture testSprite;
    public int positionCount =1; // should start at 1
    public Button bu_fillNext;
    public Button bu_fillToggle;
    Vector3 lerpPos = Vector3.zero;
    [Range(0,1)]
    float tempSpeed = 0;
    bool filling = false;

    //lr.GetPosition(i-1) / 
    public override void StartPage()
    {
        accupointsMassageManagerFinal?.StartFunc();

        bu_fillNext.onClick.AddListener(Fill);
        bu_fillToggle.onClick.AddListener(() => { filling = !filling; });
        //lr.materials[0].SetTexture("_MainTex", testSprite);
        lr.positionCount = points.Count;
        lr2.positionCount = positionCount;
    }
    //public Vector3[] GetPointsInSegment(int i)
    //{
    //    return new Vector3[] { points[i * 3].transform.position, points[i * 3 + 1].transform.position, points[i * 3 + 2].transform.position, points[LoopIndex(i * 3 + 3)].transform.position };
    //}
    public Vector3 RenderingOfGraph(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float localTValue)
    {
        var A = Vector3.Lerp(P0, P1, localTValue); // middlepoint of P0 and P1
        var B = Vector3.Lerp(P1, P2, localTValue); // middle point of P1 and P2
        var C = Vector3.Lerp(P2, P3, localTValue); // middle point of P2 and P3
        var D = Vector3.Lerp(A, B, localTValue); // middle of A and B
        var E = Vector3.Lerp(B, C, localTValue); //middle of B and C
        var P = Vector3.Lerp(D, E, localTValue); // middle of D and E 
        return P;
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (facepointsObj == null  || facepointsObj.Count <=0) return;
        if (accupointsMassageManagerFinal.gameObject.activeInHierarchy)
            accupointsMassageManagerFinal.UpdateScene();

        //for(int i = 0; i < points.Count; i++)
        //{
        //    int p = points[i];
        //    lr.SetPosition(i, facepointsObj[p].transform.position);
        //}
        //pointList.Add(RenderingOfGraph(ratio));
        //var pointList = new List<Vector3>();

        //for (int i = 0; i < segments; i++)
        //{
        //    for (float ratio = 0; ratio <= 1; ratio += 1 / 12)
        //    {
        //        Vector3[] points = GetPointsInSegment(i);

        //        pointList.Add(RenderingOfGraph(points[0], points[1], points[2], points[3], ratio));
        //    }
        //}

        //lr.SetPositions(pointList.ToArray());

        for (int i = 0;i < positionCount; i++)
        {
            //if (i == positionCount - 1)
            //{
            //    //tempSpeed = 0.2f;
            //    ////tempTime = Mathf.Clamp(tempTime, 0, 1);
            //    //Vector3 startPos = lr2.GetPosition(i);
            //    //Vector3 endPos = lr.GetPosition(i);
            //    //Vector3 vecFrom = new Vector3(0, 0, 0);
            //    //Vector3 vecTo = new Vector3(10, 10, 20);
            //    //Vector3 direction = endPos - (startPos);
            //    //Vector3 twentyPercentPoint = startPos + 0.2f * direction;
            //    //Debug.Log("start : "+ startPos);
            //    //Debug.Log("end : "+ endPos);
            //    //Debug.Log("percent : "+ twentyPercentPoint);
            //    //lr2.SetPosition(i, twentyPercentPoint);
            //}
            //else
            //{
            //}
                //lr2.SetPosition(i, lr.GetPosition(i));
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
        int lineIndex = positionCount - 1;
        Vector3 prevPos = lr2.GetPosition(lineIndex - 1);
        lr2.SetPosition(lineIndex, prevPos);
        lerpPos = lr2.GetPosition(positionCount - 2);
    }


}
