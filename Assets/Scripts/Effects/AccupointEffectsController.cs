using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Accupoint))]
public class AccupointEffectsController : MonoBehaviour
{
    [SerializeField] GameObject shrinkingSwirl;
    [SerializeField] GameObject growingSwirl;
    [SerializeField] float closingSpeedMult;
    Accupoint accupoint;
    private void Awake()
    {
        accupoint = GetComponent<Accupoint>();
    }
    public void SetClosingSpeed(float newSpeed)
    {
        closingSpeedMult = newSpeed;
    }
    private void Update()
    {
        if (accupoint.timer > accupoint.maxTimer)
        {
            GameObject newEffect = Instantiate(growingSwirl, gameObject.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
            //gameObject.SetActive(false);
            gameObject.SetActive(false);
            Destroy(newEffect, 1);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (accupoint.timer < accupoint.maxTimer && Mathf.Abs(gameObject.transform.localScale.z - 0) > float.Epsilon)
        {
            shrinkingSwirl.transform.localScale += new Vector3(-Time.deltaTime * closingSpeedMult, -Time.deltaTime * closingSpeedMult, -Time.deltaTime * closingSpeedMult);
        }
    }
}
