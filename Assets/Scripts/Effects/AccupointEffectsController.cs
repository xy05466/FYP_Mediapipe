using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Accupoint))]
public class AccupointEffectsController : MonoBehaviour
{
    [SerializeField] GameObject shrinkingSwirl;
    [SerializeField] GameObject growingSwirl;
    Accupoint accupoint;
    private void Awake()
    {
        accupoint = GetComponent<Accupoint>();
    }
    private void Update()
    {
        if (accupoint.timer > accupoint.maxTimer)
        {
            GameObject newEffect = Instantiate(growingSwirl, gameObject.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(newEffect, 1);
        }
    }
}
