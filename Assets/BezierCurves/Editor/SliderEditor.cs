using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SliderCreator))]
public class SliderEditor : Editor
{

    SliderCreator creator;

    void OnSceneGUI()
    {
        if (creator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            creator.UpdateRoad();
        }
    }

    void OnEnable()
    {
        creator = (SliderCreator)target;
    }
}
