using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Path
{
    [HideInInspector]
    public List<Vector2> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints;
    //constructor
    public Path(Vector2 centre)
    {
        Vector2 point1 = centre + Vector2.left;
        Vector2 point2 = centre + Vector2.right;

        Vector2 extraPoint1 = centre;
        Vector2 extraPoint2 = centre;

        //generating of the curve
        points = new List<Vector2>
        {
            point1,
            extraPoint1,
            extraPoint2,
            point2
        };
    }

    //for the editor script to access the points list
    public Vector2 this[int i]
    {
        get 
        { 
            return points[i]; 
        }
    }
    //accessor to getting the control points
    public bool AutoSetControlPoints
    {
        get 
        { 
            return autoSetControlPoints; 
        }
        set 
        { 
            if(autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if(autoSetControlPoints)
                {
                    //updating all control points
                    AutoSetAllControlPoints();
                }
            }
        }
    }

    //to get the number of points in the list of points
    public int NumPoints
    {
        get { return points.Count; }
    }

    //to get the number of segments currently in the path
    public int NumSegments
    {
        get
        {
            //number of points divided by 3 to find the number of segments added
            //plus for the initial segment
            //with the isclosed boolean false 1 segment = 4 points, 2 segments = 7 points, 3 segments = 10 points etc.
            //else if the isclosed boolean is true 1 segment is 6 points, 2 segments is 9 points and 3 segments is 12 points
            return points.Count / 3;
        }
    }

    //mainly to make it so that the curve is more curvy when new points are added by 'extendind' the anchor points
    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * 0.5f);
        points.Add(anchorPos);

        if(autoSetControlPoints )
        {
            //set the affected control points and the new anchor to be
            //the last element in the points list
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }
    //getting the points in a specific segment
    //based on the way this code is made, 1st segment is 4 points(initial 4 points in the constructor)
    //2nd segment is the 3 points made in the addsegment method and the last point of the 1st segment
    public Vector2[] GetPointsInSegments(int i)
    {
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3] };
    }
    //setting the position of a point
    public void MovePoint(int i, Vector2 pos)
    {
        //calculating the change in position
        Vector2 deltaMove = pos - points[i];
        //the point can only be moved if it is an anchor point
        if(i%3 ==0 || !autoSetControlPoints)
        {
            //taking the value of i and setting that to the new position
            points[i] = pos;

            //if we are also setting the control points
            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {
                //anchor point is being moved
                if (i % 3 == 0)
                {
                    if (i + 1 < points.Count || isClosed)
                        points[LoopIndex(i + 1)] += deltaMove;

                    if (i - 1 >= 0 || isClosed)
                        points[LoopIndex(i - 1)] += deltaMove;
                }
                //if it is not an anchor point
                else
                {
                    //find out whether the next point is an anchor point
                    bool NextPointIsAnchor = (i + 1) % 3 == 0;
                    //index of the corresponding control point
                    int correspondingControlIndex = (NextPointIsAnchor ? i + 2 : i - 2);
                    //index of the anchor point that the 2 control points are attached to
                    int anchorIndex = (NextPointIsAnchor) ? i + 1 : i - 1;
                    //checking that the corresponding control points exist
                    if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                    {
                        //moving of the corresponding control point but also
                        //maintaining the current distance from the anchor point
                        float dst = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                        //getting the direction from the new position of the control point being moved to the anchor point
                        Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                        points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dst;
                    }
                }
            }
        } 
    }

    public void toggleClosed()
    {
        isClosed = !isClosed;
        //if the path is closed
        if(isClosed)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add(points[0] * 2 - points[1]);
            //updating the control points of the first and last anchor points
            if(autoSetControlPoints)
            {
                AutoSetAnchorControlPoints(0);
                AutoSetAnchorControlPoints(points.Count - 3);
            }
        }
        else
        {
            points.RemoveRange(points.Count - 2, 2);
            //if the path kena toggled open
            if(autoSetControlPoints)
            {
                AutoSetStartAndEndControls();
            }
        }
    }

    public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        //adding the first point in the path to this list
        evenlySpacedPoints.Add(points[0]);
        //last point in the path being looked at
        Vector2 previousPoint = points[0];
        //keeping track of the distance travelled along the curve
        //since adding the last evenly spaced point
        float distSinceLastEvenPoint = 0;
        for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
        {
            //getting an array of points in that segment
            Vector2[] p = GetPointsInSegments(segmentIndex);
            //the distance between all the points on the curve
            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            //distance between 2 anchor points
            float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while(t <= 1) 
            {
                t += 1f/divisions;
                Vector2 pointOnCurve = BezierSpaced.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                //increasing the distance since the last even point by the distance
                //between the previous point and this new point
                distSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);
                //find the spot to put the new evenly spaced point
                //keep running until it has fitted all the evenly spaced points that it can
                while (distSinceLastEvenPoint >= spacing)
                {
                    //checking how far the distance may have been overshot
                    float overshootDst = distSinceLastEvenPoint - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    distSinceLastEvenPoint = overshootDst;
                    previousPoint = newEvenlySpacedPoint;
                }
                previousPoint = pointOnCurve;
            }
        }
        return evenlySpacedPoints.ToArray();
    }

    //param is the index of the anchor that has been changed in some way
    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex; i <= updatedAnchorIndex + 3; i+=3)
        {
            if (i >=0 && i < points.Count ||isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));

            }
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i+=3)
        {
            AutoSetAnchorControlPoints(i);
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = points[anchorIndex];
        Vector2 dir = Vector2.zero;
        //distance from the 2 anchor points to its neighbours
        float[] neighbourDistances = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            //getting offset from neighbour to current anchor
            Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
            dir += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 >= 0 || isClosed)
        {
            //getting offset from neighbour to current anchor
            Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
            dir -= offset.normalized;
            neighbourDistances[1] = -offset.magnitude;
        }

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * .5f;
            }
        }
    }

    void AutoSetStartAndEndControls()
    {
        if(!isClosed)
        {
            //finding the first control point of the open curve
            points[1] = (points[0] + points[2]) * .5f;
            //finding the last control point of the curve
            points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * .5f;
        }
    }

    //wrap around to the other end of the list
    int LoopIndex(int i)
    {
        //handling of negative values
        return (i=points.Count) % points.Count;
    }
}
