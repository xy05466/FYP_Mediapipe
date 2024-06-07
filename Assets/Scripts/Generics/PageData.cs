using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Page SO", fileName ="Page_")]
public class PageData : ScriptableObject
{
    // splash image
    public Sprite img_splashIcon;
    // title
    public string title;
    [TextArea]
    public string description;

    // annotation to enable
}

public enum AnnotationType
{
    FACE_RECOGNITION,
    HAND_RECOGNITION,
    HAND_GESTURE,
}