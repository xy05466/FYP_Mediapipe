using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlowerAccupoint : MonoBehaviour
{
    [SerializeField] GameObject slicedPetal;
    [SerializeField] GameObject pps;
    private SpriteRenderer petalSR;
    public bool collided = false;
    private float alpha;
    private void Start()
    {
        petalSR = slicedPetal.GetComponentInChildren<SpriteRenderer>();
        slicedPetal.SetActive(true);
        GameObject newParticleSystem = Instantiate(pps, transform);
        Destroy(newParticleSystem);
        alpha = this.petalSR.color.a;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Handpoint"))
        {
            collided = true; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Handpoint"))
        {
            alpha -= Time.deltaTime * 11.0f;
            petalSR.color = new Color(petalSR.color.r, petalSR.color.g, petalSR.color.b, alpha);
        }
    }
    private void Update()
    {
        //if (gameObject.transform.position.z > -10)
        //{
        //    slicedPetal.transform.localPosition = new Vector3(slicedPetal.transform.localPosition.x, slicedPetal.transform.localPosition.y, -50);
        //}
    }
}
