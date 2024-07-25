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

    [SerializeField] MaskingObjectPool Mask;
    [SerializeField] Animator animator;
    private void Start()
    {
        animator.enabled = false;
        index = bonePoints.Count - 1;
        transform.position = bonePoints.LastOrDefault().position;
        Mask.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (index >= 0)
        {
            transform.position = Vector2.Lerp(transform.position, bonePoints[index].position, Time.deltaTime);

            if (Vector3.Distance(transform.position, bonePoints[index].position) < 0.2f)
            {
                if (!coroutineRunning)
                    StartCoroutine(SpawnMasking());

                index--;
            }
        }
        if (index <= 0)
            animator.enabled = true;
    }

    IEnumerator SpawnMasking()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(0.8f);
        GameObject temp = Mask.GetObj(transform.position, transform.rotation);
        coroutineRunning = false;
    }
}
