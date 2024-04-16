using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Accupoint))]
public class AccupointEffectsController : MonoBehaviour
{
    [SerializeField] GameObject shrinkingSwirl;
    [SerializeField] GameObject growingSwirl;
    Accupoint accupoint;
    IEnumerator Wait(float i)
    {
        yield return new WaitForSeconds(i);
    }
    private void Awake()
    {
        accupoint = GetComponent<Accupoint>();
    }
    private void Update()
    {
        if (accupoint.timer > accupoint.maxTimer)
        {
            //growingSwirl.SetActive(true);
            Instantiate(growingSwirl, gameObject.transform.position, Quaternion.identity);
            Wait(1);
            gameObject.SetActive(false);
        }
    }
}
