using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{ 
    public class HelpManager : MonoBehaviour
    {
        public RaqibHandGestureManager handGestureManager;
        public GestureData gestureOpen;
        public GestureData gestureClose;

        [Header("SETTINGS")]
        public float timeLast = 2f;
        private float timeLastElapsed;

        [Header("UI")]
        public GameObject panel_help;
        public Image img_filled_open;
        public Image img_filled_close;

        private void Update()
        {
            //if (panel_help.activeInHierarchy) return;

            if(handGestureManager.GetCurrentGesture() == (panel_help.activeInHierarchy == true ? gestureClose.gestureName : gestureOpen.gestureName))
            {
                timeLastElapsed += Time.deltaTime;
                if (timeLastElapsed >= timeLast)
                {
                    timeLastElapsed = 0f;
                    panel_help.gameObject.SetActive(!panel_help.activeInHierarchy);
                }
            }
            else if(timeLastElapsed >0)
            {
                timeLastElapsed -= Time.deltaTime;
            }

            if(panel_help.activeInHierarchy)
                img_filled_close.fillAmount = timeLastElapsed / timeLast;
            else
                img_filled_open.fillAmount = timeLastElapsed / timeLast;
        }
    }
}
