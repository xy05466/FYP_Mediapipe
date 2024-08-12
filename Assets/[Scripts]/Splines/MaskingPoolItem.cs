using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskingPoolItem : MonoBehaviour
{
    private Action<MaskingPoolItem> OnInactive;
    private void OnDisable()
    {
        OnInactive?.Invoke(this);
    }
    public void Subscribe(Action<MaskingPoolItem> action)
    {
        OnInactive += action;
    }
}
