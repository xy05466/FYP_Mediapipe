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
    public Action<bool> OnParentActive;
    bool parentActive;
    protected void ParentAction()
    {
        if(parent.gameObject.activeInHierarchy && !parentActive) {
            parentActive = true;
            OnParentActive?.Invoke(true);
        }
        else if (!parent.gameObject.activeInHierarchy && parentActive)
        {
            parentActive = false;
            OnParentActive?.Invoke(false);
        }


    }
    public virtual void Update()
    {
        ParentAction();
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
    public List<T> GetPoints()
    {
        return points;
    }

    // EVENTS
    public void SubscribePointsSpawn(System.Action<List<T>> action)
    {
        OnPointsSpawned += action;
    }
    public void SubscribeParentActive(System.Action<bool> action)
    {
        OnParentActive += action;
    }
}


[System.Serializable]
public class LandmarkListGO : LandmarkList<GameObject>
{
    public override void Update()
    {
        ParentAction();
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