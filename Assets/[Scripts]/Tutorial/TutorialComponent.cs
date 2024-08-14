using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialComponent : MonoBehaviour
{
    [SerializeField] private RectTransform focusRect;
    [SerializeField] private bool focusOnActive = true;
    [TextArea()]
    [SerializeField] private string txt_instruction;
    Action<TutorialComponent> OnFocus;
    bool done = false;

    //private void Start()
    //{
    //    if (focusOnActive && gameObject.activeInHierarchy)
    //        Focus();
    //}

    private void Update()
    {
        if (focusOnActive && gameObject.activeInHierarchy)
            Focus();
    }
    public string GetInstructionText()
    {
        return txt_instruction;
    }
    public RectTransform GetTargetRect()
    {
        return focusRect == null ? GetComponent<RectTransform>() : focusRect;
    }
    public void Focus()
    {
        if (done) return;
        OnFocus?.Invoke(this);
        done = true;
    }
    public void ResetTutorialComponent()
    {
        done = false;
    }

    public void StartComponent()
    {
        if (gameObject.activeInHierarchy)
            Focus();
    }



    public void SubscribeFocus(Action<TutorialComponent> action)
    {
        OnFocus += action;
    }
}
