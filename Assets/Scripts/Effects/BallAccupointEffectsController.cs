using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BallMovement))]
public class BallAccupointEffectsController : MonoBehaviour
{
    BallMovement bm;
    [SerializeField] float growthRate = 1;
    [SerializeField] float shrinkRate = 0.5f;
    [SerializeField] float minScale = 1.5f;
    [SerializeField] float maxScale = 3.0f;
    [SerializeField] GameObject shrinkyRing;

    private void Awake()
    {
        bm = GetComponent<BallMovement>();
    }
    private void Update()
    {
        if (bm.isColliding)
        {
            GrowCircle();
        }
        else
        {
            ShrinkCircle();
        }
    }
    public void GrowCircle()
    {
        if (shrinkyRing.transform.localScale.x < maxScale)
        {
            shrinkyRing.transform.localScale += growthRate * Vector3.one * Time.deltaTime;
        }
    }
    public void ShrinkCircle()
    {
        if (shrinkyRing.transform.localScale.x > minScale && Mathf.Abs(shrinkyRing.transform.localScale.z - 0) > float.Epsilon)
        {
            shrinkyRing.transform.localScale += -Time.deltaTime * shrinkRate * Vector3.one;
        }
    }
}
