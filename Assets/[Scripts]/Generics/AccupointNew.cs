using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccupointNew : MonoBehaviour
{
    public GameObject spriteChild;
    public float timer = 3;
    public float currentTime = 0;
    bool isEnd = false;
    int accId = -1;
    bool colliding = false;
 
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
    private void Update()
    {
        if (isEnd) return;
        if (!colliding) return;
        currentTime += Time.deltaTime;
        spriteChild.transform.localScale = new Vector3(GetCalculatedScale(), GetCalculatedScale(), GetCalculatedScale());

        if (currentTime >= timer)
            isEnd = true;
    }
    public void Colliding()
    {
        colliding = true;
        Debug.Log("colliding");
 
    }
    public void AccupointReset()
    {
        currentTime = 0;
        isEnd = false;
    }

    public void StopColliding()
    {
        colliding = false;
    }
}
