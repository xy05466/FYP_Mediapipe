using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnSetComplete : MonoBehaviour
{
    private void Start()
    {
        SilderSet.onSetCompleted += DestroyObject;
    }
    private void OnDestroy()
    {
        SilderSet.onSetCompleted -= DestroyObject;

    }
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
