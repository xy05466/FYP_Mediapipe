using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class JadenCurveManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> points;
    [SerializeField]
    bool isClosed;
    [SerializeField]
    bool autoSetControlPoints;
    public GameObject pointPrefab;
    public LineRenderer LineRenderer;
    public float vertexCount = 12;


    private void Update()
    {
        var pointList = new List<Vector3>();



        //pointList.Add(RenderingOfGraph(ratio));
        for (int i = 0; i < NumSegments; i++)
        {
            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                Vector3[] points = GetPointsInSegment(i);

                pointList.Add(RenderingOfGraph(points[0], points[1], points[2], points[3], ratio));
            }
        }
        LineRenderer.positionCount = pointList.Count;
        LineRenderer.SetPositions(pointList.ToArray());
        //conditions
        if (autoSetControlPoints)
        {
            AutoSetAllControlPoints();
        }
        //if (isClosed)
        //{
        //    points.Add(returnNewPoint(points[points.Count - 1].transform.position * 2 - points[points.Count - 2].transform.position));
        //    points.Add(returnNewPoint(points[0].transform.position * 2 - points[1].transform.position));
        //    if (autoSetControlPoints)
        //    {
        //        AutoSetAnchorControlPoints(0);
        //        AutoSetAnchorControlPoints(points.Count - 3);
        //    }
        //}
        //else
        //{
        //    points.RemoveRange(points.Count - 2, 2);
        //    if (autoSetControlPoints)
        //    {
        //        AutoSetStartAndEndControls();
        //    }
        //}
    }

    public Vector3 RenderingOfGraph(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float localTValue)
    {
        var A = Vector3.Lerp(P0, P1, localTValue); // middlepoint of P0 and P1
        var B = Vector3.Lerp(P1, P2, localTValue); // middle point of P1 and P2
        var C = Vector3.Lerp(P2, P3, localTValue); // middle point of P2 and P3
        var D = Vector3.Lerp(A, B, localTValue); // middle of A and B
        var E = Vector3.Lerp(B, C, localTValue); //middle of B and C
        var P = Vector3.Lerp(D, E, localTValue); // middle of D and E 
        return P;
    }
    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return points.Count / 3;
        }
    }
    public void AddSegment(GameObject anchorPos)
    {
        points.Add(returnNewPoint(points[points.Count - 1].transform.position * 2 - points[points.Count - 2].transform.position));
        points.Add(returnNewPoint((points[points.Count - 1].transform.position + anchorPos.transform.position) * .5f));
        points.Add(anchorPos);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }

    public void SplitSegment(GameObject anchorPos, int segmentIndex)
    {
        points.InsertRange(segmentIndex * 3 + 2, new GameObject[] { returnNewPoint(Vector2.zero), anchorPos, returnNewPoint(Vector2.zero) });
        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
        }
        else
        {
            AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
        }
    }

    public void DeleteSegment(int anchorIndex)
    {
        if (NumSegments > 2 || !isClosed && NumSegments > 1)
        {
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    points[points.Count - 1] = points[2];
                }
                points.RemoveRange(0, 3);
            }
            else if (anchorIndex == points.Count - 1 && !isClosed)
            {
                points.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                points.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }
    public Vector3[] GetPointsInSegment(int i)
    {
        return new Vector3[] { points[i * 3].transform.position, points[i * 3 + 1].transform.position, points[i * 3 + 2].transform.position, points[LoopIndex(i * 3 + 3)].transform.position };
    }
    public Vector2[] GetPointsInSegmentV2(int i)
    {
        return new Vector2[] { points[i * 3].transform.position, points[i * 3 + 1].transform.position, points[i * 3 + 2].transform.position, points[LoopIndex(i * 3 + 3)].transform.position };
    }
    public void MovePoint(int i, Vector3 pos)
    {
        Vector3 deltaMove = pos - points[i].transform.position;

        if (i % 3 == 0 || !autoSetControlPoints)
        {
            points[i].transform.position = pos;

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {

                if (i % 3 == 0)
                {
                    if (i + 1 < points.Count || isClosed)
                    {
                        points[LoopIndex(i + 1)].transform.position += deltaMove;
                    }
                    if (i - 1 >= 0 || isClosed)
                    {
                        points[LoopIndex(i - 1)].transform.position += deltaMove;
                    }
                }
                else
                {
                    bool nextPointIsAnchor = (i + 1) % 3 == 0;
                    int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                    int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                    if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                    {
                        float dst = (points[LoopIndex(anchorIndex)].transform.position - points[LoopIndex(correspondingControlIndex)].transform.position).magnitude;
                        Vector3 dir = (points[LoopIndex(anchorIndex)].transform.position - pos).normalized;
                        points[LoopIndex(correspondingControlIndex)].transform.position = points[LoopIndex(anchorIndex)].transform.position + dir * dst;
                    }
                }
            }
        }
    }
    //public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    //{
    //    List<Vector2> evenlySpacedPoints = new List<Vector2>();
    //    evenlySpacedPoints.Add(points[0].transform.position);
    //    Vector2 previousPoint = points[0].transform.position;
    //    float dstSinceLastEvenPoint = 0;

    //    for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
    //    {
    //        Vector2[] p = GetPointsInSegmentV2(segmentIndex);
    //        float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
    //        float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2f;
    //        int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
    //        float t = 0;
    //        while (t <= 1)
    //        {
    //            t += 1f / divisions;
    //            Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
    //            dstSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);

    //            while (dstSinceLastEvenPoint >= spacing)
    //            {
    //                float overshootDst = dstSinceLastEvenPoint - spacing;
    //                Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
    //                evenlySpacedPoints.Add(newEvenlySpacedPoint);
    //                dstSinceLastEvenPoint = overshootDst;
    //                previousPoint = newEvenlySpacedPoint;
    //            }

    //        }
    //    }

    //    return evenlySpacedPoints.ToArray();
    //}
    //fixed code
    public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        Vector2 previousPoint = points[0].transform.position;
        evenlySpacedPoints.Add(previousPoint);
        float dstSinceLastEvenPoint = 0;

        for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
        {
            Vector2[] p = GetPointsInSegmentV2(segmentIndex);
            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);

            for (int i = 1; i <= divisions; i++)
            {
                float t = i / (float)divisions;
                Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                dstSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);

                while (dstSinceLastEvenPoint >= spacing)
                {
                    float overshootDst = dstSinceLastEvenPoint - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    dstSinceLastEvenPoint = overshootDst;
                    previousPoint = newEvenlySpacedPoint;
                }
                previousPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }
    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
        {
            if (i >= 0 && i < points.Count || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i += 3)
        {
            AutoSetAnchorControlPoints(i);
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = new Vector2(points[anchorIndex].transform.position.x, points[anchorIndex].transform.position.y);
        Vector2 dir = Vector2.zero;
        float[] neighbourDistances = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector2 offset = new Vector2(points[LoopIndex(anchorIndex - 3)].transform.position.x, points[LoopIndex(anchorIndex - 3)].transform.position.y) - anchorPos;
            dir += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 >= 0 || isClosed)
        {
            Vector2 offset = new Vector2(points[LoopIndex(anchorIndex + 3)].transform.position.x, points[LoopIndex(anchorIndex + 3)].transform.position.y) - anchorPos;
            dir -= offset.normalized;
            neighbourDistances[1] = -offset.magnitude;
        }

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                Vector2 newPosV2 = anchorPos + dir * neighbourDistances[i] * .5f;
                //Vector3 newPos = new Vector3(newPosV2.x, newPosV2.y, 0);
                //points[LoopIndex(controlIndex)].transform.position = newPosV2;
                //Vector3 currentLocalPosition;
                //currentLocalPosition = newPos;
                //points[LoopIndex(controlIndex)].transform.localPosition = currentLocalPosition.WithZ(0f);

                //Vector2 newPosV2 = anchorPos + dir * neighbourDistances[i] * .5f;
                Vector3 currentLocalPosition;
                points[LoopIndex(controlIndex)].transform.position = newPosV2;
                currentLocalPosition = points[LoopIndex(controlIndex)].transform.localPosition;
                points[LoopIndex(controlIndex)].transform.localPosition = currentLocalPosition.WithZ(0f);

            }
        }
    }

    void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1].transform.position = (points[0].transform.position + points[2].transform.position) * .5f;
            points[points.Count - 2].transform.position = (points[points.Count - 1].transform.position + points[points.Count - 3].transform.position) * .5f;
        }
    }

    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }
    private GameObject returnNewPoint(Vector3 position)
    {
        GameObject newpoint = Instantiate(pointPrefab);
        newpoint.transform.position = position;
        return newpoint;
    }
    private void OnDrawGizmos()
    {
        //draw points from point P0 to P1 and P2 to P3
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(P0.transform.position, P1.transform.position);
        //Gizmos.DrawLine(P2.transform.position, P3.transform.position);
        //balls
        for (int i = 0; i < NumSegments; i++)
        {
            Vector3[] points = GetPointsInSegment(i);

            Gizmos.color = Color.black;
            Gizmos.DrawLine(points[1], points[0]);
            Gizmos.DrawLine(points[2], points[3]);


        }
    }

}

