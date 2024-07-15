
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Pathcreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;

    public List<Vector2> points = new List<Vector2>();
    private List<Vector2> curvePoints = new List<Vector2>();
    float duration = 10.0f;
    float t = 0.0f;
    Vector2 newPos;

    public GameObject MovingBall;

    // Reset the path to a new path
    public void CreatePath()
    {
        path = new Path(transform.position);
        for (int i = 0; i < path.points.Count; i++)
        {
            points[i]  = path.points[i];
        }
    }

    void Reset()
    {
        CreatePath();
    }

    private void Start()
    {
        foreach (var point in points)
        {
            curvePoints.Add(point);
        }
        if (MovingBall == null) return;
        MovingBall.transform.position = path.points[0];
    }

    private void Update()
    {
        for (int i = 0; i < points.Count; i++)
            path.SetPointPosition(i, points[i]);


        if (t < 1.0f)
        {
            t += Time.deltaTime / duration;
            newPos = CalculateBezierPoint(t, curvePoints);
            MovingBall.transform.position = new Vector3(newPos.x, newPos.y, MovingBall.transform.position.z);
        }
   
    }

    public Vector2 CalculateBezierPoint(float t, List<Vector2> points)
    {
        Vector2 p0 = points[0];
        Vector2 p1 = points[1];
        Vector2 p2 = points[2];
        Vector2 p3 = points[3];

        //calculations for each coefficient of the formula
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        //implementation bezier curve formula
        Vector2 p = uuu * p0; // (1-t)^3 * p0
        p += 3 * uu * t * p1; // 3(1-t)^2 * t * p1
        p += 3 * u * tt * p2; // 3(1-t) * t^2 * p2
        p += ttt * p3; // t^3 * p3

        return p;
    }
}
