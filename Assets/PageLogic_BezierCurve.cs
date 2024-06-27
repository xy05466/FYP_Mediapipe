using Mediapipe.Unity.Sample;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Pathcreator))]
public class PageLogic_BezierCurve : PageLogic_Extension
{
    [SerializeField]
    List<int> points = new List<int>();
    private Dictionary<int, GameObject> spawnedHandles = new Dictionary<int, GameObject>();
    [SerializeField]
    GameObject newHandles;
    [SerializeField]
    LineRenderer lineRenderer;
    Pathcreator creator;
    Path path;
    [Range(.05f, 1.5f)]
    public float spacing = .1f;
    public float resolution = 1f;
    Vector2[] spacedPoints;

    public override void EndPage()
    {
    }

    public override bool IsPageDone()
    {
        return false;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
    }

    public override void OnHandAnnotationActive(bool active)
    {
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {

    }

    public override void ResetPage()
    {
    }


    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (facepointsObj == null || facepointsObj.Count <= 0) return;

        //spacedPoints = path.CalculateEvenlySpacedPoints(spacing);

        for (int i = 0; i < path.NumPoints; i++)
        {
            int p = points[i];
            if (i < points.Count && points[i] < facepointsObj.Count)
            {
                if (facepointsObj[p] != null)
                {
                    // Update the position of the path points to match the face points
                    path.points[i] = facepointsObj[p].transform.position;

                    // Check if we need to instantiate a new handle
                    if (!spawnedHandles.ContainsKey(i))
                    {
                        var handle = Instantiate(newHandles, path.points[i], Quaternion.identity);
                        spawnedHandles[i] = handle;
                    }
                    // Ensure the handle follows the face points
                    spawnedHandles[i].transform.position = facepointsObj[p].transform.position;
                }
            }
        }

        //lineRenderer.positionCount = spacedPoints.Length;
        //for (int i = 0; i < spacedPoints.Length; i++)
        //{
        //    lineRenderer.SetPosition(i, path.points[i]);
        //    Debug.Log(lineRenderer.GetPosition(i));
        //}

        Undo.RecordObject(creator, "Move Point");
        for (int i = 0; i < path.NumPoints; i++)
        {
            int p = points[i];
            if (facepointsObj[p] != null)
            {
                path.MovePoint(i, facepointsObj[p].transform.position);
                path.points[i] = facepointsObj[p].transform.position;
            }
        }
    }

    public override void StartPage()
    {
        //setting the creator to be equals to the editor target cast
        creator = GetComponent<Pathcreator>();
        //creating a new path if the creator is null
        if (creator.path == null)
        {
            creator.CreatePath();
        }
        //setting the path to be equals to the created path
        path = creator.path;
    }
}
