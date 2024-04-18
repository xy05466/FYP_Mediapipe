using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAccupoint : MonoBehaviour
{
    [SerializeField] GameObject slicedPetal;
    private void OnCollisionEnter(Collision collision)
    {
        slicedPetal.SetActive(true);
    }
}
