using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PartScaler : MonoBehaviour
{
    public float duration = 100f;
    public Animator animator;
    [SerializeField] MaskMoving maskMoving;
    // Start is called before the first frame update
    void Start()
    {
        animator.enabled = false;
    }

    void Update()
    {
        if (maskMoving != null && maskMoving.canScale)
        {
            animator.enabled = true;
            maskMoving.canScale = false;
        }
    }
}
