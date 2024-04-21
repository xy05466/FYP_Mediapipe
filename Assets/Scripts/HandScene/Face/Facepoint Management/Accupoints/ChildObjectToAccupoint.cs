using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObjectToAccupoint : MonoBehaviour
{
    AccupointAnchorManager Anchor;
    public GameObject parent
    {   get
        {   if (Anchor.accupoint == null)
                return null;
            return Anchor.accupoint;
        }
    }
    private void Awake()
    {
        Anchor = GetComponent<AccupointAnchorManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (parent == null)
            return; 
        //Debug.Log("Amongus ");
    }
}
