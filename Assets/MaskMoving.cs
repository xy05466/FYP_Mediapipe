using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class MaskMoving : MonoBehaviour
{
    public List<Transform> bonePoints = new List<Transform>();
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

    int tempValue = 0;
    public void PercentageMovement(float sliderValue)
    {
        int value = (bonePoints.Count - 1) - ((int)((bonePoints.Count - 1) * sliderValue));
 
        transform.position = Vector3.Lerp(transform.position, bonePoints[value].position, Time.deltaTime / bonePoints.Count);
        if (Vector3.Distance(transform.position, bonePoints[value].position) < 0.2f && tempValue != value)
        {
            tempValue = value;  
            GameObject temp = Mask.GetObj(bonePoints[tempValue].position, transform.rotation); 
        }
    }

    public void PlayAnim()
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        animator.enabled = true;
    }
}
