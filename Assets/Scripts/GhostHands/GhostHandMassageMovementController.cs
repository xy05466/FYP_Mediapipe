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
        maxTimer = newMaxTimer;
        path = newPath;
        maxSegmentTimer = maxTimer / newPath.Count;
        //Debug.Log(maxSegmentTimer);
        segmentTimer = 0;
        transform.position = newPath[0].transform.position;
    }
    IEnumerator MoveAlongPath()
    {
        float timeElapsed = 0f;
        //Debug.Log("Moving!");
        //Debug.Log(maxSegmentTimer);
        for (int i = 0; i < path.Count; ++i)
        {
            Vector3 startPos = transform.position;
            Vector3 nextPoint = path[i].transform.position;//new Vector3(path[i].transform.position.x, path[i].transform.position.y, 0);
            while (timeElapsed < maxSegmentTimer)
            {
                //Debug.Log("1");
                float t = timeElapsed / maxSegmentTimer;
                transform.position = Vector3.Lerp(startPos, nextPoint, t);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            //Debug.Log("2");
            // Ensure final position is precisely the end point
            transform.position = path[i].transform.position;
            timeElapsed = 0;
        }
        GhostHandsSceneManager.FinishedPathEvent?.Invoke();
    }
    public void StartMoving()
    {
        StartCoroutine(MoveAlongPath());
    }
}
