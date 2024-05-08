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
    public void SetMassagePath(List<GameObject> newPath, float newMaxTimer = 0)
    {
        newMaxTimer = maxTimer;
        path = newPath;
        maxSegmentTimer = maxTimer / newPath.Count;
        segmentTimer = 0;
        nextIndex = 0;
        transform.position = newPath[0].transform.position;
    }
    IEnumerator MoveAlongPath()
    {
        float timeElapsed = 0f;
        Vector3 startPos = transform.position;
        while (segmentTimer < maxSegmentTimer)
        {
            float t = timeElapsed / maxSegmentTimer;
            transform.position = Vector3.Lerp(startPos, path[nextIndex].transform.position, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // Ensure final position is precisely the end point
        transform.position = path[nextIndex].transform.position;
    }
    public void StartMoving()
    {
        StartCoroutine(MoveAlongPath());
    }
}
