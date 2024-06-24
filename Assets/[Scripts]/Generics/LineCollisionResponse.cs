using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollisionResponse : MonoBehaviour
{
    public Action<int,GameObject, Collider> OnColliding;
    private int pointID = -1;
    private string ignoreName = "";
    public void SetIgnoreName(string ignoreName)
    {
        this.ignoreName = ignoreName;
    }
    public void SetId(int id)
    {
        pointID = id;
    }
    public int GetId() { return pointID; }

    internal void Subscribe(Action<int,GameObject, Collider> collideWithLine)
    {
        OnColliding += collideWithLine;
    }
    internal void UnSubscribe(Action<int,GameObject, Collider> collideWithLine)
    {
        OnColliding -= collideWithLine;
    }
    internal void UnSubscribeAll()
    {
        OnColliding = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name != ignoreName)
        {
            OnColliding?.Invoke(pointID, gameObject, other);
        }
    }
}
