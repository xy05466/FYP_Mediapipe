using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandmarkList<T>
{
    public Transform parent;
    public string tag = "Facepoint";
    private List<T> points = new List<T>();
    private bool synced = false;
    private bool search = false;
    public System.Action<List<T>> OnHandpointsSpawned;


    public void Update()
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
                OnHandpointsSpawned?.Invoke(points);
            }
        }
    }
}
