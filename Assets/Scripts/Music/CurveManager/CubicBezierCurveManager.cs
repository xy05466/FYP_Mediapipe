using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class CubicBezierCurveManager : MonoBehaviour
{
    //t is like a percentage moves from 0 - 1;
    [Range(0.0f, 1.0f)]
    public float tValue;
    //4 poinst
    public Transform P0;
    public Transform P1;
    public Transform P2;
    public Transform P3;
    public LineRenderer LineRenderer;
    public float vertexCount = 12;
    public float[] LUT; // Declare the LUT array
    
    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        // Generate the LUT array
        GenerateLookupTable();
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
        pointList.Clear();

    }
    public Vector3 RenderingOfGraph(float localTValue)
    {
        var A = Vector3.Lerp(P0.position, P1.position, localTValue); // middlepoint of P0 and P1
        var B = Vector3.Lerp(P1.position, P2.position, localTValue); // middle point of P1 and P2
        var C = Vector3.Lerp(P2.position, P3.position, localTValue); // middle point of P2 and P3
        var D = Vector3.Lerp(A, B, localTValue); // middle of A and B
        var E = Vector3.Lerp(B, C, localTValue); //middle of B and C
        var P = Vector3.Lerp(D, E, localTValue); // middle of D and E 
        return P;
    }
   
    private void OnDrawGizmos()
    {
        //draw points from point P0 to P1 and P2 to P3
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(P0.position, P1.position);
        Gizmos.DrawLine(P2.position, P3.position);
    }

    public float DistToT(float[] LUT, float distance)
    {
        float arcLength = LUT[LUT.Length - 1]; // total arc length
        int n = LUT.Length;

        // Handle edge cases
        if (distance <= 0)
            return 0;
        else if (distance >= arcLength)
            return 1;

        // Iterate through the LUT array to find the appropriate mapping
        for (var i = 0; i < n - 1; i++)
        {
            if (distance >= LUT[i] && distance <= LUT[i + 1])
            {
                // Map distance to t within the range of LUT[i] and LUT[i + 1]
                return distance.Remap(LUT[i], LUT[i + 1], i / (n - 1f), (i + 1) / (n - 1f));
            }
        }

        // If the distance is not within any range, return a default value
        return 0;
    }
    private void GenerateLookupTable()
    {
        int resolution = 100; // Adjust the resolution as needed
        LUT = new float[resolution];
        float totalLength = 0f;
        Vector3 previousPoint = P0.position;

        for (int i = 1; i < resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 currentPoint = RenderingOfGraph(t);
            totalLength += Vector3.Distance(previousPoint, currentPoint);
            LUT[i] = totalLength;
            previousPoint = currentPoint;
        }
    }
    
}


