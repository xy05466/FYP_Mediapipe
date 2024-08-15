using System;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class Acupoint : MonoBehaviour
    {
        public GameObject spriteChild;
        public SliderSprite sliderSprite;
        public float timer = 3;
        public float currentTime = 0;
        bool isEnd = false;
        int accId = -1;
        public Action<Acupoint> OnFinished;
        private float timeout = 0.1f;
        private float timeoutElapsed = 0f;

        private void Update()
        {
            if (isEnd) return;
            if (timeoutElapsed > timeout) return;
            timeoutElapsed += Time.deltaTime;
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
                if (sliderSprite)
                    sliderSprite.Fill(currentTime / timer);
            }
        }
        public void Initialize(float timer = 3)
        {
            this.timer = timer;
            spriteChild.transform.localScale = new Vector3(GetCalculatedScale(), GetCalculatedScale(), GetCalculatedScale());
        }
        public void Initialize() => Initialize(timer);

        public void SetID(int accId)=> this.accId = accId;
        public int GetAccID() => accId;
        float GetCalculatedScale() => timer - currentTime;

        public void Colliding()
        {
            timeoutElapsed = 0f;
        }
        public void AccupointReset()
        {
            currentTime = 0;
            isEnd = false;
        }
        // EVENTS
        public void Subscribe(Action<Acupoint> action) => OnFinished += action;
        public void Unsubscribe(Action<Acupoint> action) => OnFinished -= action;

    }
}