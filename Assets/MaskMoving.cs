using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MaskMoving : MonoBehaviour
{
    public List<Transform> bonePoints = new List<Transform>();
    int index = 0;
    bool coroutineRunning = false;
    SpriteRenderer spriteRenderer;
    [SerializeField] MaskingObjectPool Mask;
    [SerializeField] Animator animator;
    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        animator.enabled = false;
        transform.position = bonePoints.LastOrDefault().position;
        Mask.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnMasking()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(0.3f);
        GameObject temp = Mask.GetObj(transform.position, transform.rotation);
        coroutineRunning = false;
    }

    public void PercentageMovement(float sliderValue)
    {
        float multiplier = 1.0f;

        if (sliderValue >= 0.75f)
        {
            multiplier = 0.25f;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            animator.enabled = true;
        }
        else if (sliderValue >= 0.5f)
        {
            multiplier = 0.5f;
        }
        else if (sliderValue >= 0.25f)
        {
            multiplier = 0.75f;
        }

        int value = Mathf.CeilToInt(bonePoints.Count * multiplier);
        if (value >= 0)
        {
            transform.position = Vector3.Lerp(transform.position, bonePoints[value].position, Time.deltaTime);
            if (Vector3.Distance(transform.position, bonePoints[value].position) < 0.2f)
            {
                if (!coroutineRunning)
                {
                    StartCoroutine(SpawnMasking());
                }
            }
        }
    }
}
