using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{

    public Image m_Image;

    public bool playAwake = true;
    public float playerAfter = 1;
    public bool m_reverse = false;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;

    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;

    private void Start()
    {
        if (playAwake)
            Invoke("PlayUIAnim", playerAfter);
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        Func_PlayUIAnim();
    //    }
    //}
    public void PlayUIAnim()
    {
        IsDone = false;
        if (m_reverse)
            m_IndexSprite = m_SpriteArray.Length - 1;
        else
            m_IndexSprite = 0;

        m_Image.sprite = m_SpriteArray[m_IndexSprite];

        StartCoroutine(PlayAnimUI());
    }
    public void PlayUIAnim(bool reverse)
    {
        m_reverse = reverse;
        PlayUIAnim();
    }

    public void StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(PlayAnimUI());
    }
    IEnumerator PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if(!m_reverse)
        {
            if (m_IndexSprite >= m_SpriteArray.Length)
            {
                //m_IndexSprite = 0;
                StopUIAnim();
            }
        }
        else
        {
            if (m_IndexSprite <= 0)
            {
                //m_IndexSprite = m_SpriteArray.Length-1;
                StopUIAnim();
            }
        }


        if (!m_reverse)
        {
            m_IndexSprite += 1;
        }
        else
        {
            m_IndexSprite -= 1;
        }
        if (!IsDone)
            m_Image.sprite = m_SpriteArray[m_IndexSprite];

        if (IsDone == false)
            m_CorotineAnim = StartCoroutine(PlayAnimUI());
    }
}