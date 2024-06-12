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
        alpha = petalSR.color.a;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Handpoint"))
        {
            collided = true;
            if (collided)
            {
                alpha -= Time.deltaTime * 20.0f;
                //Debug.Log(petalSR);
                petalSR.color = new Color(petalSR.color.r, petalSR.color.g, petalSR.color.b, alpha);
                //StartCoroutine(FadeOut());
            }
        }
        else
        {
            collided = false;
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
