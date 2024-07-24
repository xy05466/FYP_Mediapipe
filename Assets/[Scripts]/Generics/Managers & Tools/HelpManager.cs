using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    public RaqibHandGestureManager handGestureManager;
    public GestureSO gesture;
    [Header("SETTINGS")]
    public float timeLast = 2f;
    private float timeLastElapsed;

    [Header("UI")]
    public GameObject panel_help;
    public Image img_filled;

    private void Update()
    {
        //if (panel_help.activeInHierarchy) return;

        if(handGestureManager.GetCurrentGesture() == gesture.gestureName)
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

        img_filled.fillAmount = timeLastElapsed / timeLast;
    }
}
