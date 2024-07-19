using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName ="Page SO", fileName ="PageData_")]
public class PageData : ScriptableObject
{
    public bool enableHeader= true;
    public bool autoNavigate = false;
    [Header("Splash Data")]
    public bool enableTransition = true;
    public bool enableSplashPage = true;
    // splash image
    public Sprite spr_splashIcon;
    // title
    public string title;
    public string titleSub;
    [TextArea]
    public string splashDescription;

    [Header("Trackings")]
    // annotation to enable
    public bool enableFaceTracking;
    public bool enableHandTracking;

    //[Header("Trackings")]
    //public PageLogic pageLogic;
}

//public enum AnnotationType
//{
//    FACE_RECOGNITION,
//    HAND_RECOGNITION,
//    HAND_GESTURE,
//}

#if UNITY_EDITOR
//public class PageDataEditor : editor
#endif