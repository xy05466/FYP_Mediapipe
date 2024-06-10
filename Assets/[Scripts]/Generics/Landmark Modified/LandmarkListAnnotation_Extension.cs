using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkListAnnotation_Extension : MonoBehaviour
{ 
    private List<GameObject> facepoints = new List<GameObject>();
    private bool synced = false;
    private bool search = false;
    public System.Action<List<GameObject>> OnHandpointsSpawned;

    private void Update()
    {
        if (!synced)
        {
            foreach (var item in GetComponentsInChildren<Transform>())
            {
                if (item.gameObject.CompareTag(tag))
                {
                    facepoints.Add(item.gameObject);
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
