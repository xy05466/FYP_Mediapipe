using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MassageSliderObjectV2 : MonoBehaviour
{
    public LineRenderer lr_outline;
    public LineRenderer lr_filled;
    public int startLandmark;
    public int endLandmark;
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
        lr_outline.positionCount = 2;
        lr_filled.positionCount = 2;
    }
    // will move the slider
    public void UpdateSlider(GameObject compareObject, float distance = 1f)
    {
        //if (!this.move) return;
        Vector3 startpoint = lr_outline.GetPosition(0);
        Vector3 endpoint = lr_outline.GetPosition(1);
        Vector3 valuePosition = GetValuePosition();
        if (Vector3.Distance(compareObject.transform.position, valuePosition) <= distance)
        {
            Debug.Log("IM HERE");
            // check if compare position is more than the current value position
            //float checkDistance = Vector3.Distance(startPoint, compareObject.transform.position);
            sliderValue += Time.deltaTime;
            sliderValue = Mathf.Clamp(sliderValue, 0f, 1f);
        }

        //// Calculate the distance from start to the check position
        //float checkDistance = Vector3.Distance(startPoint, compareObject.transform.position);

        //// Calculate the total distance
        //float totalDistance = Vector3.Distance(startPoint, endpoint);

        //if (checkDistance > totalDistance)
        //    massageSliderObject.FillCurrentPoint(1 / totalDistance);
    }
    public void UpdatePosition(Vector3 posStart, Vector3 posEnd)
    {
        lr_outline.SetPosition(0, posStart);
        lr_outline.SetPosition(1, posEnd);

        lr_filled.SetPosition(0, posStart);
        lr_filled.SetPosition(1, GetValuePosition());
    }
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
        Vector3 v = lr_outline.GetPosition(0) + sliderValue * (lr_outline.GetPosition(1) - lr_outline.GetPosition(0));

        return v;
    }
    public void ResetSlider()
    {
        sliderValue = 0f;
        end = false;
    }
}
