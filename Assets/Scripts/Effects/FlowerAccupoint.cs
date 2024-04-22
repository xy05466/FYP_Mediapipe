using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAccupoint : MonoBehaviour
{
    [SerializeField] GameObject slicedPetal;
    [SerializeField] GameObject pps;
    private void OnCollisionEnter(Collision collision)
    {
        slicedPetal.SetActive(true);
        //if (pps != null)
        //{
        //    GameObject newGO = Instantiate(pps, transform);
        //    newGO.transform.localPosition = gameObject.transform.localPosition;
        //}
    }
    private void Update()
    {
        if (gameObject.transform.position.z > -10)
        {
            slicedPetal.transform.localPosition = new Vector3(slicedPetal.transform.localPosition.x, slicedPetal.transform.localPosition.y, -50);
        }
    }
}
