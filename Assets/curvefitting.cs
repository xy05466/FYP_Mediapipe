using UnityEngine;
using System.Collections.Generic;
using Accord.Math;

public class CurveFitting : MonoBehaviour
{
    public string dataPointTag = "DataPoint"; // Tag used to find data point GameObjects
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    List<UnityEngine.Vector3> dataPoints;

    void Start()
    {
        // Get data points from GameObjects with the specified tag
        dataPoints = GetDataPointsFromGameObjects(dataPointTag);

        // Fit a third-degree polynomial to the data points
        float[] coefficients = FitPolynomial(dataPoints, 2);

        // Plot the fitted polynomial curve using the LineRenderer
        PlotFittedCurve(coefficients);
    }

    /// <summary>
    /// Finds all GameObjects with the specified tag and collects their positions.
    /// </summary>
    /// <param name="tag">Tag used to find GameObjects</param>
    /// <returns>List of Vector3 positions from the GameObjects</returns>
    List<UnityEngine.Vector3> GetDataPointsFromGameObjects(string tag)
    {
        // Find all GameObjects with the specified tag
        GameObject[] dataPointObjects = GameObject.FindGameObjectsWithTag(tag);

        // List to store the positions
        List<UnityEngine.Vector3> points = new List<UnityEngine.Vector3>();

        // Collect the position of each GameObject
        foreach (GameObject obj in dataPointObjects)
        {
            points.Add(obj.transform.position);
        }

        return points;
    }

    /// <summary>
    /// Fits a polynomial of the specified degree to the given data points.
    /// </summary>
    /// <param name="points">List of Vector3 data points</param>
    /// <param name="degree">Degree of the polynomial</param>
    /// <returns>Array of polynomial coefficients</returns>
    float[] FitPolynomial(List<UnityEngine.Vector3> points, int degree)
    {
        int n = points.Count; // Number of data points
        int m = degree + 1; // Number of coefficients

        // Create matrix A and vector b for the linear system
        double[,] A = new double[n, m];
        double[] b = new double[n];

        // Fill matrix A with powers of x and vector b with y values
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                A[i, j] = Mathf.Pow(points[i].x, j);
            }
            b[i] = points[i].y;
        }

        // Solve for coefficients using the normal equations method
        double[,] AT = A.Transpose(); // Transpose of A
        double[,] ATA = AT.Dot(A); // ATA = AT * A
        double[] ATb = AT.Dot(b); // ATb = AT * b

        // Solve the linear system ATA * coefficients = ATb
        double[] coefficients = ATA.Solve(ATb);

        // Convert the coefficients to float array
        float[] result = new float[m];
        for (int i = 0; i < m; i++)
        {
            result[i] = (float)coefficients[i];
        }

        return result;
    }

    /// <summary>
    /// Plots the fitted polynomial curve using the LineRenderer.
    /// </summary>
    /// <param name="coefficients">Array of polynomial coefficients</param>
    void PlotFittedCurve(float[] coefficients)
    {
        // Determine the range of x values from the data points
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        // Find the minimum and maximum x values from the data points
        foreach (var point in dataPoints)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
        }

        int resolution = 1000; // Number of points to plot on the curve
        List<UnityEngine.Vector3> curvePoints = new List<UnityEngine.Vector3>(); // List to store the curve points

        // Generate points on the curve using the polynomial coefficients
        for (int i = 0; i < resolution; i++)
        {
            // Calculate the interpolation factor t
            float t = i / (float)(resolution - 1);
            // Interpolate the x value within the range of data points
            float x = Mathf.Lerp(minX, maxX, t);
            float y = 0;

            // Calculate the y value using the polynomial equation
            for (int j = 0; j < coefficients.Length; j++)
            {
                y += coefficients[j] * Mathf.Pow(x, j);
            }

            // Add the point to the list
            curvePoints.Add(new UnityEngine.Vector3(x, y, 0));
        }

        // Set the positions of the LineRenderer to the curve points
        lineRenderer.positionCount = curvePoints.Count;
        lineRenderer.SetPositions(curvePoints.ToArray());
    }
}
