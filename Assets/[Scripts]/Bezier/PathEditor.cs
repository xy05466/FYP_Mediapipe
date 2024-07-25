using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
#if UNITY_EDITOR
[CustomEditor(typeof(Pathcreator))]
public class PathEditor : Editor
{
    Pathcreator creator;
    Path path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        //manually creating a path
        if (GUILayout.Button("Create new"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreatePath();
            path = creator.path;
        }

        if (GUILayout.Button("Toggle Closed"))
        {
            Undo.RecordObject(creator, "Toggle Closed");
            path.toggleClosed();
        }

        //toggle for auto setting the control points pool
        bool autoSetControlPoints = GUILayout.Toggle(path.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != path.AutoSetControlPoints)
        {
            //record with undo button
            Undo.RecordObject(creator, "Toggle auto set controls");
            path.AutoSetControlPoints = autoSetControlPoints;
        }
        EditorGUI.EndChangeCheck();

        //checking if anything has changed
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }
    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(creator, "Add segment");
            path.AddSegment(mousePos);
        }
    }

    //to create handles in order to move around each of the points
    public void Draw()
    {
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] points = path.GetPointsInSegments(i);
            Handles.color = Color.black;
            //lines going from control points to their respective anchor points
            //Handles.DrawLine(points[0], points[3]);
            //first 2 points is start and end of curve the last 2 points is to handle the curve(but not needed)
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
        }

        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            // Returns a vector3 for the new position of the handles if they have been moved
            var fmh_82_75_638573468304152528 = Quaternion.identity; Vector2 newPosHandle = Handles.FreeMoveHandle(path.points[i], .1f, Vector2.zero, Handles.CylinderHandleCap);

            // Checking if the position has changed
            if (path[i] != newPosHandle)
            {
                Undo.RecordObject(creator, "Move Point");
                path.MovePoint(i, newPosHandle);
                //newPosHandle = facepoints[i].transform.position;
            }
        }
    }

    private void OnEnable()
    {
        //setting the creator to be equals to the editor target cast
        creator = (Pathcreator)target;
        //creating a new path if the creator is null
        if (creator.path == null)
        {
            creator.CreatePath();
        }
        //setting the path to be equals to the created path
        path = creator.path;
    }
}
#endif