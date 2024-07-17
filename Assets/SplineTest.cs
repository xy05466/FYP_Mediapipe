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
    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform EndPoint;
    private void Start()
    {
        //returns the length of the spline in world space
        splineLength = splines.CalculateLength();
        transform.position = StartPoint.position;
    }

    private void Update()
    {
        distancePercentage += speed * Time.deltaTime / splineLength;

        //evaluates a position of a point on the spline curve
        Vector3 currentPosition = splines.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        if (transform.position == StartPoint.position/*EndPoint.position + offset || transform.position == EndPoint.position - offset*/)
        {
            Debug.Log("asdsd");
        }
        //Vector3 nextPosition = splines.EvaluatePosition(distancePercentage + 0.03f);
        //Vector3 direction = nextPosition - currentPosition;


        //transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}
