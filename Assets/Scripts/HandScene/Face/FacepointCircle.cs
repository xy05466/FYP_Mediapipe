using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacepointCircle : MonoBehaviour
{
    [SerializeField] float startScale = 1.0f;
    [SerializeField] float shrinkSpeed = 1.0f;
    [SerializeField] GameObject torus;
    private void Awake()
    {
        torus.transform.localPosition = new Vector3(0, 0, -3);  
    }
    private void Update()
    {
        if (torus.transform.localScale.z > -1)
        {
            torus.transform.localScale = torus.transform.localScale - (new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime);
        }
        else
        {
            Destroy(torus);
        }
    }
}
