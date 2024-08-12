using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Scriptable/Gesture Data", fileName ="Gesture Data")]
public class GestureData : ScriptableObject 
{
    public string gestureName;
    [EnumData(typeof(GestureFinger))]
    public GestureFingerValue[] fingerDistances = new GestureFingerValue[Enum.GetValues(typeof(GestureFinger)).Length];
}

[System.Serializable]
public class GestureFingerValue
{
    public float minDistance;
    public float threshold;
}
public enum GestureFinger
{
    THUMB,
    INDEX,
    MIDDLE,
    RING,
    PINKY,
}

