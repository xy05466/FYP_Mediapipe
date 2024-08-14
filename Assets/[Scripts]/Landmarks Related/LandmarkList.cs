using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandmarkList<T>
{
    public Transform parent;
    public string tag = "Facepoint";
    protected int startIndex = 0;
    protected List<T> points = new List<T>();
    protected bool loaded = false;
    protected bool search = false;
    public System.Action<List<T>> OnPointsSpawned;
    public Action<bool> OnParentActive;
    bool parentActive;
    public void SetStartIndex(int startIndex)
    {
        this.startIndex = startIndex;
    }
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
        if (!loaded)
        {
            Transform[] listTransform = parent.GetComponentsInChildren<Transform>(true);
            int count = 0;

            foreach (Transform item in listTransform)
            {
                if (item.gameObject.CompareTag(tag))
                {
                    count++;
                    if (count >= startIndex)
                    {
                        points.Add(item.gameObject.GetComponent<T>());
                        if (!search)
                            search = true;
                    }
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
            Transform[] listTransform = parent.GetComponentsInChildren<Transform>(true);
            int count = 0;
          
            foreach (Transform item in listTransform)
            {
                if (item.gameObject.CompareTag(tag))
                {
                    count++;
                    if(count >= startIndex)
                    {
                        points.Add(item.gameObject);
                        if (!search)
                            search = true;
                    }
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