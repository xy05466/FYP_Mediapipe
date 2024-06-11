using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAccupoint : MonoBehaviour
{
    [SerializeField] GameObject slicedPetal;
    [SerializeField] GameObject pps;
    private SpriteRenderer sr;
    public bool collided = false;
    float time;
    float alpha;
    private void Start()
    {
        sr = slicedPetal.GetComponent<SpriteRenderer>();
        slicedPetal.SetActive(true);
        GameObject newParticleSystem = Instantiate(pps, transform);
        Destroy(newParticleSystem);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Handpoint"))
        {
            if (!collided)
            {
                collided = true;
                sr.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                alpha -= Time.deltaTime * 2;
                if (collided && alpha <= 0.0f)
                {
                    Debug.Log("one done");
                    collided = false;
                    Destroy(collision.gameObject);
                }
            }
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
