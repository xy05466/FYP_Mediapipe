using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccupointNew : MonoBehaviour
{
    public GameObject spriteChild;
    public SliderSprite sliderSprite;
    public float timer = 3;
    public float currentTime = 0;
    bool isEnd = false;
    int accId = -1;
    public Action<AccupointNew> OnFinished;
 
    public void Initialize(float timer = 3)
    {
        this.timer = timer;
        spriteChild.transform.localScale = new Vector3 (GetCalculatedScale(), GetCalculatedScale(), GetCalculatedScale());
    }
    public void Initialize()
    {
        Initialize(timer);
    }
    public void SetID(int accId)
    {
        this.accId = accId;
    }

    public int GetAccID()
    {
        return accId;
    }
    float GetCalculatedScale()
    {
        return timer - currentTime;
    }
  
    public void Colliding()
    {
        if (isEnd) return;
        currentTime += Time.deltaTime;
        spriteChild.transform.localScale = new Vector3(GetCalculatedScale(), GetCalculatedScale(), GetCalculatedScale());

        if (currentTime >= timer)
        {
            isEnd = true;
            if (sliderSprite)
                sliderSprite.ResetSlider();
            OnFinished?.Invoke(this);
        }
        else
        {
            if(sliderSprite)
                sliderSprite.Fill(currentTime / timer);
        }
    }
    public void AccupointReset()
    {
        currentTime = 0;
        isEnd = false;
    }
    public void Subscribe(Action<AccupointNew> action)
    {
        OnFinished += action;
    }
    public void Unsubscribe(Action<AccupointNew> action)
    {
        OnFinished -= action;
    }

}
