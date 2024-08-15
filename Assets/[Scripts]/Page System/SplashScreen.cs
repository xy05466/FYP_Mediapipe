using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class SplashScreen : MonoBehaviour
    {
        public HandGestureManager handGestureManager;
        public GestureData gestureNavigate;
        public Button bu_close;
        public float duration;
        public CanvasGroup canvasGroup;
        private Coroutine coroutine;

        public Image img_splashIcon;
        public TMP_Text tmp_description;
        [Header("Gesture")]
        public float navigateTime = 2f;
        private float navigateTimeElapsed = 0f;
        public Image img_gesture_filled;

        private void Start()
        {
            if (!canvasGroup)
                canvasGroup = GetComponent<CanvasGroup>();

            bu_close.onClick.AddListener(StartFadeOut);
        }
        private void Update()
        {
            if (handGestureManager != null && gestureNavigate != null && coroutine == null)
            {
                if (handGestureManager.GetCurrentGesture() == gestureNavigate.gestureName)
                {
                    navigateTimeElapsed += Time.deltaTime;
                }
                else
                {
                    navigateTimeElapsed -= Time.deltaTime;
                }
                navigateTimeElapsed = Mathf.Clamp(navigateTimeElapsed, 0f, navigateTime);
                if (navigateTimeElapsed >= navigateTime)
                {
                    Debug.Log("Done");
                    StartFadeOut();
                    navigateTimeElapsed = 0f;
                }
                if (img_gesture_filled)
                    img_gesture_filled.fillAmount = navigateTimeElapsed / navigateTime;
            }
        }
        public void StartFadeOut()
        {
            if (coroutine != null)
                return;

            coroutine = StartCoroutine(IeFadeOut());
        }

        IEnumerator IeFadeOut()
        {
            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                //Fade from 1 to 0
                float alpha = Mathf.Lerp(1, 0, counter / duration);

                canvasGroup.alpha = alpha;
                //Wait for a frame
                yield return null;
            }
            this.gameObject.SetActive(false);
            coroutine = null;
        }

        public void ShowSplash()
        {
            if (canvasGroup) 
                canvasGroup.alpha = 1;
            gameObject.SetActive(true);
        }
    }
}