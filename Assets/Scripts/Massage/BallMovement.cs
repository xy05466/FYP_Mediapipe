using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public JadenCurveManager path;

    public bool isColliding;
    private Vector2[] points;
    private int currentIndex = 0;

    float elapsed = 0f;
    public float duration = 0.5f;
    void Update()
    {
        points = path.CalculateEvenlySpacedPoints(0.1f);
        //for(var pointsInCount = 0; points.Length < pointsInCount )
        if (!isColliding)
            return;
        //else if(points == null)
        elapsed += Time.deltaTime;
        if (currentIndex < points.Length - 1 )
        {
            // Move the ball towards the current point
            transform.position = Vector3.Lerp(points[currentIndex], points[currentIndex + 1], elapsed / duration);
            //move on to next point
            if (elapsed >= duration)
            {
                currentIndex++;
                elapsed = 0;
            }
        }
        //transform.localPosition.WithZ(0f);
        Vector3 currentLocalPosition;
        currentLocalPosition = transform.localPosition;
        transform.localPosition = currentLocalPosition.WithZ(0f);
    }
    //handle collision
    private void OnTriggerStay(Collider other)
    {
        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;

    }

}
