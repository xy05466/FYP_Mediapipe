using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class SplineMovement : MonoBehaviour
{
    [SerializeField] SplineGenerator splineGenerator;
    public SplineContainer splines;
    [SerializeField] private MaskingObjectPool mask;
    bool coroutineRunning2 = false;
    [SerializeField] private float speed = 0f;
    private float distancePercentage = 0f;
    private void Start()
    {
        if (!splineGenerator.End)
            return;

        mask.Initialize();
    }

    private void Update()
    {
        if (splineGenerator.index == splineGenerator.bezierKnots.Count)
        {
            movement();
            DistanceCheck();
            if (!coroutineRunning2)
            {
                StartCoroutine(SpawnMasking());
            }
        }
    }

    protected virtual void movement()
    {
        distancePercentage += speed * Time.deltaTime / splineGenerator.splineLength;
        //evaluates a position of a point on the spline curve
        Vector3 currentPosition = splines.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;
    }

    protected virtual void DistanceCheck()
    {
        if (distancePercentage > 0.98f)
        {
            distancePercentage = 0f;
            splineGenerator.End = true;
        }
    }

    IEnumerator SpawnMasking()
    {
        coroutineRunning2 = true;
        yield return new WaitForSeconds(0.8f);
        GameObject temp = mask.GetObj(transform.position, transform.rotation);
        coroutineRunning2 = false;
    }
}
