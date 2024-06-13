using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateOnCurve : MonoBehaviour
{
    //need to calculate time to take to travel from one point to the other 
    public CubicBezierCurveManager curveManager;
    public float speed = 1.0f;

    public float distanceTraveled = 0.0f;

    void Update()
    {
        // Calculate distance to move based on speed and time
        //float distanceToMove = speed * Time.deltaTime;

        // Map distance to corresponding t value
        float tValue = curveManager.DistToT(curveManager.LUT, distanceTraveled);

        // Calculate character position using cubic Bezier curve equation
        Vector3 characterPosition = curveManager.RenderingOfGraph(tValue);

        // Update character position
        transform.position = characterPosition;
        // Ensure distanceTravelled stays within the range [0, arcLength]
        //distanceTraveled = Mathf.Clamp(distanceTraveled, 0f, curveManager.LUT[curveManager.LUT.Length - 1]);
        // Update distance traveled
        //distanceTraveled += distanceToMove;
    }
}
