using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHandMassageMovementController : MonoBehaviour
{
    public List<GameObject> path;
    public float maxTimer;
    float maxSegmentTimer;
    float segmentTimer;
    int nextIndex;
    public bool isDone;
    public void SetMassagePath(List<GameObject> newPath)
    {
        path = newPath;
        maxSegmentTimer = maxTimer / newPath.Count;
        segmentTimer = 0;
        nextIndex = 0;
    }
    IEnumerator MoveAlongPath()
    {
        for (int i = 0; i < path.Count; ++i) {
            Vector3 newPos = Vector3.Lerp(transform.position, path[nextIndex].transform.position, segmentTimer / maxSegmentTimer);
            transform.position = newPos;
            segmentTimer += Time.deltaTime;

            if (segmentTimer > maxSegmentTimer)
            {
                if (nextIndex < path.Count - 1)
                {
                    nextIndex++;
                    segmentTimer = 0;
                }
                else
                {
                    isDone = true;
                }
            }
            yield return null;
        }
    }
    public void StartMoving()
    {
        StartCoroutine(MoveAlongPath());
    }
}
