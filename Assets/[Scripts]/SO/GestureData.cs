using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Scriptable/Gesture", fileName ="Gesture Data")]
public class GestureData : ScriptableObject 
{
  public string gestureName;
  public List<Vector2> fingerDistances;
  public GestureFingerValue[] fingerDistances2 = new GestureFingerValue[Enum.GetValues(typeof(GestureFinger)).Length];
}

[System.Serializable]
public class GestureFingerValue
{
    public float x;
    public float y;
}
public enum GestureFinger
{
    THUMB,
    INDEX,
    MIDDLE,
    RING,
    PINKY,
}

