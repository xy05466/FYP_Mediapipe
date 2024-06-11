using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandmarkList<T>
{
    public Transform parent;
    public string tag = "Facepoint";
    protected List<T> points = new List<T>();
    protected bool synced = false;
    protected bool search = false;
    public System.Action<List<T>> OnPointsSpawned;


    public virtual void Update()
    {
        if (!synced)
        {
            foreach (var item in parent.GetComponentsInChildren<Transform>())
            {
                if (item.gameObject.CompareTag(tag))
                {
                    points.Add(item.gameObject.GetComponent<T>());
                    if (!search)
                        search = true;
                }
            }
            if (search)
            {
                synced = true;
                OnPointsSpawned?.Invoke(points);
            }
        }
    }
}


[System.Serializable]
public class LandmarkListGO : LandmarkList<GameObject>
{

    public override void Update()
    {
        if (!synced)
        {
            foreach (var item in parent.GetComponentsInChildren<Transform>())
            {
                if (item.gameObject.CompareTag(tag))
                {
                    points.Add(item.gameObject);
                    if (!search)
                        search = true;
                }
            }
            if (search)
            {
                synced = true;
                OnPointsSpawned?.Invoke(points);
            }
        }
    }
}