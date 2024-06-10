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
    public System.Action<int,GameObject> OnColliding;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tagCollider))
        {
            PointAnnotation annotation = other.GetComponent<PointAnnotation>();
            annotation.SetColor(colorColliding);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(tagCollider))
        {
            PointAnnotation annotation = other.gameObject.GetComponent<PointAnnotation>();
            PointIndex pointIndex = other.gameObject.GetComponent<PointIndex>();
            annotation.SetColor(colorColliding);
            OnColliding?.Invoke(pointIndex.GetIndex(),other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(tagCollider))
        {
            //Debug.Log("hand colliding");
            //PointAnnotation annotation = other.gameObject.GetComponent<PointAnnotation>();
            //MeshRenderer mr = other.gameObject.GetComponent<MeshRenderer>();
            //mr.material.color = colorDefault;
            //annotation.SetColor(colorDefault);
            PointAnnotation annotation = other.gameObject.GetComponent<PointAnnotation>();
            annotation.SetColor(colorDefault);
        }
    }
   
}
