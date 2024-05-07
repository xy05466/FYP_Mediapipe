using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHandMassageMovementController : MonoBehaviour
{
    public List<Vector3> path;
    public float maxTimer;
    float maxSegmentTimer;
    float segmentTimer;
    int nextIndex;
    void SetMassagePath(List<Vector3> newPath)
    {
        path = newPath;
        maxSegmentTimer = maxTimer / newPath.Count;
        segmentTimer = 0;
        nextIndex = 0;
    }
    void MoveAlongPath(float deltaTime)
    {
        Vector3.Lerp(transform.position, path[nextIndex], segmentTimer / maxSegmentTimer);
        segmentTimer += deltaTime;

        if (segmentTimer > maxSegmentTimer && (nextIndex < path.Count -1))
        {
            nextIndex++;
            segmentTimer = 0;
        }
    }
    private void Update()
    {
        MoveAlongPath(Time.deltaTime);
    }
}
