using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PartScaler : MonoBehaviour
{
    private Vector3 ogScale = Vector3.zero;
    private Vector3 newScale = Vector3.zero;
    public float duration = 100f;
    public Animator animator;
    [SerializeField] MaskMoving maskMoving;
    // Start is called before the first frame update
    void Start()
    {
        ogScale = transform.localScale;
        animator.enabled = false;
        transform.localScale = newScale;
    }

    void Update()
    {
        if (maskMoving != null && maskMoving.canScale)
        {
            StartCoroutine(ScaleToOriginal());
        }
    }

    IEnumerator ScaleToOriginal()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            newScale = Vector3.Lerp(newScale, ogScale, elapsedTime / duration);
            transform.localScale = newScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = ogScale;
        animator.enabled = true;
        maskMoving.canScale = false;
    }
}
