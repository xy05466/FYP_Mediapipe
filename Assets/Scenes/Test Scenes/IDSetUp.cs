using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDSetUp : MonoBehaviour
{
    Transform[] childObject;
    private void Start()
    {
        childObject = GetComponentsInChildren<Transform>();

        for (int i = 1; i < childObject.Length; i++)
        {
            childObject[i].name = (i-1).ToString();
    }
    }
}
