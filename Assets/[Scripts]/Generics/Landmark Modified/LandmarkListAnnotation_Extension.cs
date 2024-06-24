using System.Collections.Generic;
using UnityEngine;

public class LandmarkListAnnotation_Extension<T> : MonoBehaviour
{ 
    private List<T> facepoints = new List<T>();
    private bool synced = false;
    private bool search = false;
    public System.Action<List<T>> OnHandpointsSpawned;

    private void Update()
    {
        if (!synced)
        {
            foreach (var item in GetComponentsInChildren<Transform>())
            {
                if (item.gameObject.CompareTag(tag))
                {
                    facepoints.Add(item.gameObject.GetComponent<T>());
                    if (!search)
                        search = true;
                }
            }
            if (search)
            {
                synced = true;
                OnHandpointsSpawned?.Invoke(facepoints);
            }
        }
    }
}
