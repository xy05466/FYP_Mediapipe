using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
public class SplineTest : MonoBehaviour
{
    public SplineContainer splines;
    private float distancePercentage = 0f;
    [SerializeField] private float speed = 0f;
    private float splineLength;
    private bool End;
    public List<BezierKnot> bezierKnots = new List<BezierKnot>();
    Spline spline = new Spline();
    private  int index = 0;
    bool coroutineRunning = false;
    private bool isMoving2 = false;

    private void Start()
    {
        InstantSpline();
    }

    private void Update()
    {
        if (End)
            return;

        CoroutineCheck();

        if (isMoving2)
        {
            movement();
            DistanceCheck();
        }
    }

    protected virtual void movement()
    {
        distancePercentage += speed * Time.deltaTime / splineLength;
        //evaluates a position of a point on the spline curve
        Vector3 currentPosition = splines.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;
    }

    protected virtual void InstantSpline()
    {
        splines.AddSpline(spline);
    }

    protected virtual void CoroutineCheck()
    {
        if (index < bezierKnots.Count && !coroutineRunning)
        {
            StartCoroutine(instantiateMasking());
        }
        DistanceCalc();
    }

    protected virtual void DistanceCheck()
    {
        if (distancePercentage > 0.98f)
        {
            distancePercentage = 0f;
            End = true;
        }
    }

    protected virtual void DistanceCalc()
    {
        //returns the length of the spline in world space
        splineLength = splines.CalculateLength();
    }

    protected virtual IEnumerator instantiateMasking()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(1f);

        spline.Add(bezierKnots[index]);
        spline.SetTangentMode(TangentMode.AutoSmooth);
        movement();
        index++;
        coroutineRunning = false;
    }
}
