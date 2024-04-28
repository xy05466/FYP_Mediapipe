using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubicBezierCurveManager : MonoBehaviour
{
    //t is like a percentage moves from 0 - 1;
    [Range(0.0f, 1.0f)]
    public float tValue;
    //4 poinst
    public GameObject P0;
    public GameObject P1;
    public GameObject P2;
    public GameObject P3;
    public LineRenderer LineRenderer;
    public float vertexCount = 12;
    Vector2 MaxRange, MinRange;
    private void Start()
    {
        tValue = 1;
        MaxRange = FindPoint();
        tValue = 0;
        MinRange = FindPoint();
    }
    private void Update()
    {
        var pointList = new List<Vector3>();

        for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        {

            pointList.Add(RenderingOfGraph(ratio));
        }
        LineRenderer.positionCount = pointList.Count;
        LineRenderer.SetPositions(pointList.ToArray());
    }
    public Vector3 RenderingOfGraph(float localTValue)
    {
        var A = Vector3.Lerp(P0.transform.position, P1.transform.position, localTValue); // middlepoint of P0 and P1
        var B = Vector3.Lerp(P1.transform.position, P2.transform.position, localTValue); // middle point of P1 and P2
        var C = Vector3.Lerp(P2.transform.position, P3.transform.position, localTValue); // middle point of P2 and P3
        var D = Vector3.Lerp(A, B, localTValue); // middle of A and B
        var E = Vector3.Lerp(B, C, localTValue); //middle of B and C
        var P = Vector3.Lerp(D, E, localTValue); // middle of D and E 
        return P;
    }
    public Vector2 FindPoint()
    {
        Vector2 graph;
        graph.x = calculatingPoint(P0.transform.position.x, P1.transform.position.x, P2.transform.position.x, P3.transform.position.x);
        graph.y = calculatingPoint(P0.transform.position.y, P1.transform.position.y, P2.transform.position.y, P3.transform.position.y);
        Debug.Log(graph.x + " , " + graph.y);
        return graph;
    }
    public float calculatingPoint(float P0, float P1, float P2, float P3)
    {
        float pos;
        pos = P0 * (Pow(-tValue, 3) + 3 * Pow(tValue, 2) - 3 * tValue + 1) +
                P1 * (3 * Pow(tValue, 3) - 6 * Pow(tValue, 2) + 3 * tValue) +
                P2 * (-3 * Pow(tValue, 3) + 3 * Pow(tValue, 2)) +
                P3 * (Pow(tValue, 3));
        return pos;
    }
    //public Vector4 boundingBox()
    //{
    //    var tX = quadraticEquation(P0.transform.position.x, P1.transform.position.x, P2.transform.position.x, P3.transform.position.x);
    //    var tY = quadraticEquation(P0.transform.position.y, P1.transform.position.y, P2.transform.position.y, P3.transform.position.y);
    //    float[] xPoints;
    //    float[] yPoints;
    //    xPoints = new float[4];
    //    yPoints = new float[4];
    //    //compare points with P0 and P3
    //    //get max min X Y
    //    var P0x = P0.transform.position.x;
    //    var P0y = P0.transform.position.y;
    //    var P3x = P3.transform.position.x;
    //    var P3y = P3.transform.position.y;
    //    for (var i = 0; i < 2; i++)
    //    {
    //        if (!tX[i].valid)
    //            continue;
    //        tX[i].valid = RangeCheck(tX[i].value, true);
    //        if (!tX[i].valid)
    //            continue;
    //        xPoints[i] = tX[i].value;
    //    }
    //    for (var i = 0; i < 2; i++)
    //    {
    //        if (!tY[i].valid)
    //            continue;
    //        tY[i].valid = RangeCheck(tY[i].value, false);
    //        if (!tY[i].valid)
    //            continue;
    //        yPoints[i] = tY[i].value;
    //    }

    //    xPoints[2] = P0x;
    //    xPoints[3] = P3x;
    //    yPoints[2] = P0y;
    //    yPoints[3] = P3y;
    //    for (var i = 0; i < 4; i++)
    //    {
    //        Debug.Log("tx = " + i + " " + xPoints[i]);
    //        Debug.Log("ty = " + i + " " + yPoints[i]);
    //    }
    //    //Debug.Log("tx0 = " + tX[0]);
    //    //Debug.Log("tx1 = " + );
    //    //Debug.Log("ty0 = " + ty0);
    //    //Debug.Log("ty1 = " + ty1);
    //    //Debug.Log("P0x = " + P0x);
    //    //Debug.Log("P0y = " + P0y);
    //    //Debug.Log("P3x = " + P3x);
    //    //Debug.Log("P3y = " + P3y);

    //    var minX = xPoints.Min();
    //    var maxX = xPoints.Max();
    //    var minY = yPoints.Min();
    //    var maxY = yPoints.Max();

    //    return new Vector4(minX, maxX, minY, maxY);

    //}
    public Vector4 boundingBox()
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        {
            Vector3 point = RenderingOfGraph(ratio);
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }

        return new Vector4(minX, maxX, minY, maxY);
    }

    float Pow(float a, float b)
    {
        float result = 1;
        for (int i = 0; i < b; i++)
        {
            result *= a;
        }
        return result;
    }
    private bool RangeCheck(float value, bool isX)
    {
        if (isX)
        {
            if (value <= MaxRange.x && value >= MinRange.x)
                return true;
            else
                return false;
        }
        else
        {
            if (value <= MaxRange.y && value >= MinRange.y)
                return true;
            else
                return false;
        }
    }
    private Point[] quadraticEquation(float P0, float P1, float P2, float P3)
    {
        Point[] points;
        points = new Point[2];
        points[0] = new Point();
        points[1] = new Point();
        var a = -3 * P0 + 9 * P1 - 9 * P2 + 3 * P3;
        var b = 6 * P0 - 12 * P1 + 6 * P2;
        var c = -3 * P0 + 3 * P1;
        var squareRoot = Mathf.Sqrt(Pow(b, 2) - (4 * a * c));
        points[0].value = ((-b + squareRoot) / (2 * a));
        points[1].value = ((-b - squareRoot) / (2 * a));
        Debug.Log("squareRoot = " + squareRoot);
        if (squareRoot < 0)
        {
            points[0].valid = false;
            points[1].valid = false;
        }
        return points;
    }
    private void OnDrawGizmos()
    {
        //draw points from point P0 to P1 and P2 to P3
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(P0.transform.position, P1.transform.position);
        Gizmos.DrawLine(P2.transform.position, P3.transform.position);
        var boundingBoxvalues = boundingBox();
        var minX = boundingBoxvalues.x;
        var maxX = boundingBoxvalues.y;
        var minY = boundingBoxvalues.z;
        var maxY = boundingBoxvalues.w;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((new Vector3(maxX, maxY, 0) * 0.5f + new Vector3(minX, minY, 0) * 0.5f), new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), 0));
    }
    public float DistToT(float[] LUT, float distance)
    {
        float arcLength = LUT[LUT.Length - 1]; // total arc length
        int n = LUT.Length;

        //if(distance.Between(0,arcLength))
        if (distance > 0 && distance < arcLength)
        {
            for (var i = 0; i < n - 1; i++)
            {
                return distance.Remap(
                    LUT[i],
                    LUT[i + 1],
                    i / (n - 1f),
                    (i + 1) / (n - 1f)
                     );
            }
        }

        return distance / arcLength;
    }
}

public class Point
{
    public bool valid = true;
    public float value;
}
public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}