using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_Essence : PageLogic_Extension
{
    public FacepointManager_Jaden facepointManager_Jaden;
    public AccupointsMassageManagerFinal accupointsMassageManagerFinal;


    public List<int> points = new List<int>();
    private int currentIndex = 0;
    public MassageSliderObject massageSliderObject;
    [Header("TESTING point")]

    public float sliderfloat = 0.5f;
    //lr.GetPosition(i-1) / 

    public float colliderWidth = 0.1f;
    public override void StartPage()
    {
        //accupointsMassageManagerFinal?.StartFunc();

        //lr.materials[0].SetTexture("_MainTex", testSprite);
        massageSliderObject.lr_outline.positionCount = points.Count;
        massageSliderObject.lr_filled.positionCount = currentIndex+1;
        massageSliderObject.Subscribe(CollideWithLine);
        massageSliderObject.UpdateColliders();
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (facepointsObj == null || facepointsObj.Count <= 0) return;
        //if (accupointsMassageManagerFinal.gameObject.activeInHierarchy)
        //    accupointsMassageManagerFinal.UpdateScene();
        massageSliderObject.lr_outline.positionCount = points.Count;
        massageSliderObject.lr_filled.positionCount = currentIndex + 2;
        for (int i = 0; i < points.Count; i++)
        {
            int p = points[i];
            massageSliderObject.lr_outline.SetPosition(i, facepointsObj[p].transform.position);
        }
        if(currentIndex < massageSliderObject.lr_outline.positionCount)
        // set collider position
        for (int i = 0; i < massageSliderObject.lr_outline.positionCount - 1; i++)
        {
            massageSliderObject.PositionColliderBetweenPoints(i, massageSliderObject.lr_outline.GetPosition(i), massageSliderObject.lr_outline.GetPosition(i + 1));
        }

        //fill line renderer
        for (int i = 0; i < massageSliderObject.lr_filled.positionCount; i++)
        {
            if(i== massageSliderObject.lr_filled.positionCount - 1)
            {
                Vector3 startPosition = massageSliderObject.lr_outline.GetPosition(i-1);
                Vector3 endPosition = massageSliderObject.lr_outline.GetPosition(i);

                // direction of vector from end to start
                Vector3 direction = endPosition - startPosition;

                // Calculate percentage of vector
                Vector3 percentagePosition = startPosition + direction * sliderfloat;
                // Set positions in lr2
                massageSliderObject.lr_filled.SetPosition(i, percentagePosition);
            }
            else
            {
                massageSliderObject.lr_filled.SetPosition(i, massageSliderObject.lr_outline.GetPosition(i));
            }

        }

    }
    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
        if(currentIndex >=points.Count) return;
        //if (points[currentIndex] == facePointIndex)
        //{
        //    currentIndex++;
        //    sliderfloat = 0;
        //}
        //if (currentIndex > 0 && points[currentIndex - 1] == facePointIndex)
        //{
        //    sliderfloat += Time.deltaTime;
        //    sliderfloat = Mathf.Clamp(sliderfloat, 0.0f, 1.0f);
        //}
        
        // if collide with current facepoint
        if (CollideWithFacePoint(points[currentIndex],facePointIndex))
        {
            //FillCurrentPoint();
        }
    }
    void FillCurrentPoint(float fill)
    {
        if (currentIndex >=points.Count) return;

        // to do:  Utilize fill variable

        sliderfloat += Time.deltaTime;
        sliderfloat = Mathf.Clamp(sliderfloat, 0.0f, 1.0f);
        if (sliderfloat >= 1)
            NextPoint();
    }
    void NextPoint()
    {
        sliderfloat = 0;
        currentIndex++;
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

    void CollideWithLine(int id, GameObject goObject, Collider other)
    {
        if (currentIndex >= points.Count) return;
        if (other.gameObject.tag == "Handpoint" && id == currentIndex)
        {
            Vector3 startPoint = massageSliderObject.lr_outline.GetPosition(currentIndex);
            Vector3 endpoint = massageSliderObject.lr_outline.GetPosition(currentIndex + 1);
            // check 
            Debug.Log("startPoint "+ startPoint);
            Debug.Log("Closest point :" +other.ClosestPoint(goObject.transform.position));
            Debug.Log("lr current point :" + massageSliderObject.lr_filled.GetPosition(massageSliderObject.lr_filled.positionCount-1));
            Debug.Log("endpoint " + endpoint);

            // Calculate the total distance
            float totalDistance = Vector3.Distance(startPoint, massageSliderObject.lr_filled.GetPosition(massageSliderObject.lr_filled.positionCount - 1));

            // Calculate the distance from start to the check position
            float checkDistance = Vector3.Distance(startPoint, other.gameObject.transform.position);

            Debug.Log("totalDistance " + totalDistance);
            Debug.Log("checkDistance " + checkDistance);

            if(checkDistance > totalDistance)
                FillCurrentPoint(1);
        }
    }

}

[Serializable]
public class SliderStep
{
    public List<int> points = new List<int>();
}

