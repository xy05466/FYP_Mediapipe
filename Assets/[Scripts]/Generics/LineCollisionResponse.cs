using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollisionResponse : MonoBehaviour
{
    public Action<GameObject,GameObject> OnCollide;

    internal void Subscribe(Action<GameObject, GameObject> collideWithLine)
    {
        OnCollide += collideWithLine;
    }
    internal void UnSubscribe(Action<GameObject, GameObject> collideWithLine)
    {
        OnCollide -= collideWithLine;
    }
    internal void UnSubscribeAll()
    {
        OnCollide = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnCollide?.Invoke(gameObject,other.gameObject);
    }
}
