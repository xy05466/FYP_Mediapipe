using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MassageSliderObjectV2 : MonoBehaviour
{
    [Header("Interacting object")]
    public GameObject targetObject;
    private SpriteRenderer rendererObject;
    public Color color_default = Color.white;
    public Color color_interacting = Color.green;
    public Color color_done = Color.gray;
    //public Sprite spr_default;
    //public Sprite spr_interacting;
    //public Sprite spr_done;

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

    //public Action<MassageSliderObjectV2, int, GameObject, Collider> OnColliding;

    private float sliderValue = 0f;
    private float targetValue = 0f;
    bool end = false;
    bool move = false;
    public Action<MassageSliderObjectV2> OnEnded;

    private void Awake()
    {
        rendererObject = targetObject.GetComponent<SpriteRenderer>();
    }

    // Set up start end points
    public void SetUp()
    {
        //startLandmark = start;
        //endLandmark = end;
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
    bool within = false;
    int tempCompIndex = 0;
    // will move the slider
    public void UpdateSlider(GameObject compareObject, float distance = 1f,float speed = 1f,float firstEnterDistance = 0.05f)
    {
        if (end) return;
        // set the position
        for (int i = 0; i < segmentCount; i++)
        {
            float percentage = ((float)i / segmentCount);
            lr_outline.SetPosition(i, CalculateBezierPoint(percentage, pathPoints) + Vector3.forward * -1f);
        }

        Vector3 valuePosition = GetValuePosition();
        targetObject.transform.position = valuePosition + Vector3.forward*-1f;

        float comp = ((float)(tempCompIndex + 1) / (float)lr_outline.positionCount);

        if(targetValue < comp)
        {
            if (Vector2.Distance(compareObject.transform.position, GetTargetPosition()) <= distance)
            {
                targetValue = comp;
            }
        }
        else
        {
            tempCompIndex++;
        }
        //for (int i = 0; i < lr_outline.positionCount; i++)
        //{
        //    //float comp = ((float)(i + 1) / (float)lr_outline.positionCount);


        //    if (targetValue < comp && i < lr_outline.positionCount)
        //    {
        //        Vector3 segmentPos = lr_outline.GetPosition(i);
        //        float distBetwSeg = Vector3.Distance(segmentPos, compareObject.transform.position);
        //        if (distBetwSeg <= distance)
        //        {
        //            Debug.Log("target : " + targetValue + " compare : " + comp);
        //            targetValue = comp;
        //        }
        //    }
        //}

        //float distanceTemp;

        //if(within)
        //    distanceTemp = distance;
        //else
        //    distanceTemp = firstEnterDistance;


        //// interacting
        //if (Vector2.Distance(compareObject.transform.position, GetTargetPosition()) <= distanceTemp)
        //{
        //    within = true;
        //    targetValue += Time.deltaTime * speed;
        //    targetValue = Mathf.Clamp(targetValue, 0f, 1f);
        //    //float currentVal = GetValueFromtStartingPos(GetValuePosition());
        //    //float myVal = Vector3.Distance(compareObject.transform.position,);

        //    //if (myVal >= targetValue)
        //    //{
        //    //    targetValue = myVal;
        //    //}
        //    //// fill up line
        //    //if (sliderValue > percentageList[fillCount])
        //    //{
        //    //    fillCount++;
        //    //    //lr_filled.positionCount = fillCount;
        //    //}



        //    //rendererObject.color = color_interacting;

        //}
        //else
        //{
        //    within = false;
        //    //rendererObject.color = color_default;
        //}

        // if completed
        if (targetValue >= 1f)
        {
            end = true;
            Debug.Log("ending");
            rendererObject.color = color_done;
            OnEnded?.Invoke(this);
        }

        sliderValue = Mathf.Lerp(sliderValue, targetValue, 5f * Time.deltaTime);

        //if(Vector3.Distance(GetValuePosition(),GetTargetPosition()) > 0.01f)
        //{
        //    rendererObject.color = color_interacting;
        //}
        //else
        //{
        //    rendererObject.color = color_default;
        //}

        // Set fill position base on outline position
        for (int i = 0; i < lr_filled.positionCount; i++)
            lr_filled.SetPosition(i, lr_outline.GetPosition(i));

    }
    //public void UpdatePosition(Vector3 posStart, Vector3 posEnd)
    //{
    //    lr_outline.SetPosition(0, posStart);
    //    lr_outline.SetPosition(1, posEnd);

    //    lr_filled.SetPosition(0, posStart);
    //    lr_filled.SetPosition(1, GetValuePosition());
    //}

    //public void MoveSlider(bool move)
    //{
    //    this.move = move;
    //}

    public Vector3 GetValuePosition()
    {
        //// This is for linear path
        //Vector3 v = lr_outline.GetPosition(0) + sliderValue * (lr_outline.GetPosition(1) - lr_outline.GetPosition(0));
        //return v;

        // This is for bezier path
        return CalculateBezierPoint(sliderValue, pathPoints);
    }

    public Vector3 GetTargetPosition()
    {
        // This is for bezier path
        return CalculateBezierPoint(targetValue, pathPoints);
    }
    public void ResetSlider()
    {
        sliderValue = 0f;
        targetValue = 0f;
        end = false;
        lr_filled.positionCount = 0;
        rendererObject.color = color_default;
        tempCompIndex = 0;
    }
    public float GetValue()
    {
        return sliderValue;
    }

    // Calculated bezier path, done by Bryant Toh
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

    public void SubscribeEvent_Ended(Action<MassageSliderObjectV2> action)
    {
        OnEnded += action;
    }
}
