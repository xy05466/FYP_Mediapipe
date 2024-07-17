using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
public class SplineTest : MonoBehaviour
{
    public SplineContainer splines;
    private float distancePercentage = 0f;
    [SerializeField] private float speed = 0f;
    private float splineLength;
    private bool End;
    private void Start()
    {
        //returns the length of the spline in world space
        splineLength = splines.CalculateLength();
    }

    private void Update()
    {
        if (End)
            return;

        distancePercentage += speed * Time.deltaTime / splineLength;
        //evaluates a position of a point on the spline curve
        Vector3 currentPosition = splines.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 0.98f)
        {
            distancePercentage = 0f;
            End = true;
        }
        //Vector3 nextPosition = splines.EvaluatePosition(distancePercentage + 0.03f);
        //Vector3 direction = nextPosition - currentPosition;


        //transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}
