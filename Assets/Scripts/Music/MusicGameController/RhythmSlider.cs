using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class RhythmSlider : MonoBehaviour
{
    public List<Transform> P;

    public LineRenderer LineRenderer;
    public float vertexCount = 12;
    public float[] LUT; // Declare the LUT array
    public float speed = 1.0f;
    private bool init = false;
    
    private void Start()
    {
         LineRenderer = GetComponent<LineRenderer>();   
    }
    public void Init()
    {
        // Generate the LUT array
        GenerateLookupTable();
        init = true;
    }
    private void Update()
    {
        if (!init)
            return;

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
        var A = Vector3.Lerp(P[0].position, P[1].position, localTValue); // middlepoint of P0 and P1
        var B = Vector3.Lerp(P[1].position, P[2].position, localTValue); // middle point of P1 and P2
        var C = Vector3.Lerp(P[2].position, P[3].position, localTValue); // middle point of P2 and P3
        var D = Vector3.Lerp(A, B, localTValue); // middle of A and B
        var E = Vector3.Lerp(B, C, localTValue); //middle of B and C
        var FinalP = Vector3.Lerp(D, E, localTValue); // middle of D and E 
        return FinalP;
    }

    private void OnDrawGizmos()
    {
        //draw points from point P0 to P1 and P2 to P3
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(P[0].position, P[1].position);
        Gizmos.DrawLine(P[2].position, P[3].position);
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
        Vector3 previousPoint = P[0].position;

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
public class Point
{
    public bool valid = true;
    public float value;
}