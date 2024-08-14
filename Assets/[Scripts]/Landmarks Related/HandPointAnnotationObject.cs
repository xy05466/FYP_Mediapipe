using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandPointAnnotationObject : MonoBehaviour
{
    public string tagCollider = "Facepoint";
    public Color colorColliding = Color.green;
    public Color colorDefault = Color.white;
    public System.Action<int,GameObject,int, GameObject> OnCollidingFacePoint; // hand point collide with face point
}
