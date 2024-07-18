using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Splines;

public class SplineTest2 : SplineTest
{
    private bool isMoving = false;
    //bool coroutineRunning2 = false;
    //int index2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        base.InstantSpline();
    }

    // Update is called once per frame
    void Update()
    {
        base.CoroutineCheck();

        if (!isMoving)
            return;

        base.movement();
        base.DistanceCheck();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Mask"))
    //    {
    //        isMoving = true;
    //    }
    //}
}
