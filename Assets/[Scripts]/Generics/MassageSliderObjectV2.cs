using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MassageSliderObjectV2 : MonoBehaviour
{
    public GameObject testGO;
    public LineRenderer lr_outline;
    public LineRenderer lr_filled;
    private int fillCount = 0;
    public List<Vector3> pathPoints = new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
    [Range(4,30)]
    public int segmentCount = 10;
    public int startLandmark;
    public int endLandmark;
    List<float> percentageList = new List<float>();
    //public List<int> points = new List<int>();
    //private float sliderValue = 0f;
    //private int currentIndex = 0;

    public Action<MassageSliderObjectV2, int, GameObject, Collider> OnColliding;

    private float sliderValue = 0f;
    bool end = false;
    bool move = false;
    public Action OnEnded;
    // Set up start end points
    public void SetUp(int start, int end)
    {
        startLandmark = start;
        endLandmark = end;
        lr_outline.positionCount = segmentCount;
        lr_filled.positionCount = 0;

        for (int i = 0; i < segmentCount; i++)
        {
            float percentage = ( (float)i / segmentCount);
            percentageList.Add(percentage);
            lr_outline.SetPosition(i,CalculateBezierPoint(percentage, pathPoints));
        }
    }
    public void SetPointPosition(int index, Vector3 pos)
    {
        pathPoints[index] = pos;
    }
    // will move the slider
    public void UpdateSlider(GameObject compareObject, float distance = 1f)
    {
        // set the position
        for (int i = 0; i < segmentCount; i++)
        {
            float percentage = ((float)i / segmentCount);
            lr_outline.SetPosition(i, CalculateBezierPoint(percentage, pathPoints));
        }


        Vector3 valuePosition = GetValuePosition();
        testGO.gameObject.transform.position = valuePosition;

        if (Vector2.Distance(compareObject.transform.position, valuePosition) <= distance)
        {

            // set fill position count
            //lr_filled.positionCount = fillCount;

            sliderValue += Time.deltaTime * 0.2f;
            sliderValue = Mathf.Clamp(sliderValue, 0f, 1f);

            if(sliderValue>.5)
            {
                Debug.Log("IM HERE " + valuePosition);
                lr_filled.positionCount = segmentCount / 2;
            }
        }

        for (int i = 0; i < lr_filled.positionCount; i++)
        {
            lr_filled.SetPosition(i, lr_outline.GetPosition(i));
        }

        //Vector3 startpoint = lr_outline.GetPosition(0);
        //Vector3 endpoint = lr_outline.GetPosition(1);
        //Vector3 valuePosition = GetValuePosition();
        //if (Vector3.Distance(compareObject.transform.position, valuePosition) <= distance)
        //{
        //    Debug.Log("IM HERE");
        //    // check if compare position is more than the current value position
        //    //float checkDistance = Vector3.Distance(startPoint, compareObject.transform.position);
        //    sliderValue += Time.deltaTime;
        //    sliderValue = Mathf.Clamp(sliderValue, 0f, 1f);
        //}

        //// Calculate the distance from start to the check position
        //float checkDistance = Vector3.Distance(startPoint, compareObject.transform.position); 

        //// Calculate the total distance
        //float totalDistance = Vector3.Distance(startPoint, endpoint);

        //if (checkDistance > totalDistance)
        //    massageSliderObject.FillCurrentPoint(1 / totalDistance);
    }
    //public void UpdatePosition(Vector3 posStart, Vector3 posEnd)
    //{
    //    lr_outline.SetPosition(0, posStart);
    //    lr_outline.SetPosition(1, posEnd);

    //    lr_filled.SetPosition(0, posStart);
    //    lr_filled.SetPosition(1, GetValuePosition());
    //}
    public float GetDistanceFromValuePosition(Vector3 pos)
    {
        return Vector3.Distance(pos, GetValuePosition());
    }
    //public void MoveSlider(bool move)
    //{
    //    this.move = move;
    //}

    public Vector3 GetValuePosition()
    {
        //Vector3 v = lr_outline.GetPosition(0) + sliderValue * (lr_outline.GetPosition(1) - lr_outline.GetPosition(0));

        //return v;

        return CalculateBezierPoint(sliderValue, pathPoints);
    }
    public void ResetSlider()
    {
        sliderValue = 0f;
        end = false;
    }

    public Vector3 CalculateBezierPoint(float t, List<Vector3> points)
    {
        Vector3 p0 = points[0];
        Vector3 p1 = points[1];
        Vector3 p2 = points[2];
        Vector3 p3 = points[3];

        //calculations for each coefficient of the formula
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        //implementation bezier curve formula
        Vector3 p = uuu * p0; // (1-t)^3 * p0
        p += 3 * uu * t * p1; // 3(1-t)^2 * t * p1
        p += 3 * u * tt * p2; // 3(1-t) * t^2 * p2
        p += ttt * p3; // t^3 * p3

        return p;
    }
}
