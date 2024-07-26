using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandmarkList<T>
{
    public Transform parent;
    public string tag = "Facepoint";
    protected List<T> points = new List<T>();
    public int startFromIndex = 0;
    protected bool loaded = false;
    protected bool search = false;
    public System.Action<List<T>> OnPointsSpawned;
    public Action<bool> OnParentActive;
    bool parentActive;
    protected void ParentAction()
    {
        if(!loaded) return;
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
        if (!loaded)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>();
            int count = 0;
            //foreach (var item in transforms)
            for (int i = 0; i < transforms.Length; i++)
            {
                Transform item = transforms[i];
                if (item.gameObject.CompareTag(tag))
                {
                    if(count>= startFromIndex)
                    {
                        points.Add(item.gameObject.GetComponent<T>());
                        if (!search)
                            search = true;
                    }
                    count++;
                }
            }
            if (search)
            {
                loaded = true;
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
    public bool isLoaded()
    {
        return loaded;
    }
}


[System.Serializable]
public class LandmarkListGO : LandmarkList<GameObject>
{
    public override void Update()
    {
        ParentAction();
        if (!loaded)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>();
            int count = 0;
            //foreach (var item in transforms)
            for (int i = 0; i < transforms.Length; i++)
            {
                Transform item = transforms[i];
                if (item.gameObject.CompareTag(tag))
                {
                    if (count >= startFromIndex)
                    {
                        points.Add(item.gameObject);
                        if (!search)
                            search = true;
                    }
                    count++;
                }
            }
            if (search)
            {
                loaded = true;
                OnPointsSpawned?.Invoke(points);
            }
        }
    }
}