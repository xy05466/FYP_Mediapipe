using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class TransitionPage_TextureAnimation : TransitionPage
    {
        public Image m_Image;

        public bool playAwake = true;
        public float playerAfter = 0;
        public bool loop = false;
        public bool m_reverse = false;
        public Sprite[] m_SpriteArray;
        public float m_Speed = .02f;

        private int m_IndexSprite;
        Coroutine m_CorotineAnim;
        bool isDone = true;


        private void Start()
        {
            m_Image.enabled = false;
            if (playAwake)
                Invoke("PlayUIAnim", playerAfter);
        }

        public void PlayUIAnim()
        {
            m_Image.enabled = true;
            isDone = false;
            if (m_reverse)
                m_IndexSprite = m_SpriteArray.Length - 1;
            else
                m_IndexSprite = 0;

            m_Image.sprite = m_SpriteArray[m_IndexSprite];

            m_CorotineAnim = StartCoroutine(IePlayAnimUI());
        }
        public void PlayUIAnim(bool reverse)
        {
            m_reverse = reverse;
            PlayUIAnim();
        }
        public bool IsDone()
        {
            return isDone;
        }
        public void StopUIAnim()
        {
            m_Image.enabled = false;
            isDone = true;
            StopCoroutine(m_CorotineAnim);
            OnAnimationFinished?.Invoke();
            m_CorotineAnim = null;
        }
        IEnumerator IePlayAnimUI()
        {
            yield return new WaitForSeconds(m_Speed);
            if (!m_reverse)
            {
                if (m_IndexSprite >= m_SpriteArray.Length - 1)
                {
                    //m_IndexSprite = 0;
                    if (!loop)
                        StopUIAnim();
                    else
                        m_IndexSprite = 0;
                }
            }
            else
            {
                if (m_IndexSprite <= 0)
                {
                    //m_IndexSprite = m_SpriteArray.Length-1;
                    if (!loop)
                        StopUIAnim();
                    else
                        m_IndexSprite = m_SpriteArray.Length - 1;
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

            if (!isDone)
                m_Image.sprite = m_SpriteArray[m_IndexSprite];

            if (isDone == false)
            {
                m_CorotineAnim = StartCoroutine(IePlayAnimUI());

            }
        }
    }
}