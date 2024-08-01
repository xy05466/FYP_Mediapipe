using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
public class SplineGenerator : MonoBehaviour
{
    public SplineContainer splines;
    [HideInInspector]
    public float splineLength = 0f;
    [HideInInspector]
    public bool End;
    public List<BezierKnot> bezierKnots = new List<BezierKnot>();
    Spline spline = new Spline();
    [HideInInspector]
    public int index = 0;
    bool coroutineRunning = false;

    private void Start()
    {
        InstantSpline();
    }

    private void Update()
    {
        if (End)
            return;

        CoroutineCheck();
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

    void DistanceCalc()
    {
        //returns the length of the spline in world space
        splineLength = splines.CalculateLength();
    }

    IEnumerator instantiateMasking()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(1f);

        spline.Add(bezierKnots[index]);
        spline.SetTangentMode(TangentMode.AutoSmooth);
        index++;
        coroutineRunning = false;
    }


}
