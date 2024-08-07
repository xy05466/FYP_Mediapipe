using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Testlandmark : AnnotationController<HierarchicalAnnotation>
{
    //public List<GameObject> points = new List<GameObject>();
    //bool synced = false;
    //public System.Action<List<GameObject>> OnHandpointsSpawned;

    //protected abstract void SyncNow()
    //{
    //    base.SyncNow();
    //    if (!synced)
    //    {
    //        Debug.Log("sync once");
    //        synced = true;
    //        foreach (var item in GetComponentsInChildren<Transform>())
    //        {
    //            if (item.gameObject.CompareTag("Handpoint"))
    //            {
    //                points.Add(item.gameObject);
    //            }
    //        }
    //        OnHandpointsSpawned?.Invoke(points);
    //    }
    //}

}

public class DSD : Testlandmark
{
    protected override void SyncNow()
    {
        throw new System.NotImplementedException();
    }
}