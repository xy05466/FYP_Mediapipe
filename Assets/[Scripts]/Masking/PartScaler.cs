using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PartScaler : MonoBehaviour
{
    private Vector3 ogScale = Vector3.zero;
    private Vector3 newScale = Vector3.zero;
    public float duration = 100f;
    public Animator animator, budAnimator;
    bool coroutineEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        ogScale = transform.localScale;
        budAnimator.enabled = false;
        transform.localScale = newScale;
    }

    void Update()
    {
        if (animator.isActiveAndEnabled)
        {
            StartCoroutine(ScaleToOriginal());
            budAnimator.enabled = true;
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
    }
}
