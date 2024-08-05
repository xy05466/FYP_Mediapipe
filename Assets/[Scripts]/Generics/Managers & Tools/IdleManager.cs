using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdleManager : MonoBehaviour
{
    [Header("MANAGERS")]
    [SerializeField] private LandmarkManager landmarkManager;
    [SerializeField] private RaqibHandGestureManager handGestureManager;
    [Space]
    [SerializeField] private Image img_filled_continue;
    [SerializeField] private float timeoutTime = 3f;
    private float timeoutTimeElapsed = 0f;
    [SerializeField] private GameObject panel_pause;
    [SerializeField] private GestureSO gestureContinue;
    bool idle = false;

    private void Start()
    {
        landmarkManager.SubscribeFacePointsActive(OnFaceActive);
    }
    private void Update()
    {
        if (panel_pause.activeInHierarchy)
        {
            if(handGestureManager.GetCurrentGesture()== gestureContinue.gestureName)
            {
                if (timeoutTimeElapsed >= timeoutTime)
                {
                    panel_pause.gameObject.SetActive(false);
                    timeoutTimeElapsed = 0f;
                }
                else
                {
                    timeoutTimeElapsed += Time.deltaTime;
                }
            }
            else if (timeoutTimeElapsed > 0f)
            {
                timeoutTimeElapsed -= Time.deltaTime;
            }
            return;
        }
        if (idle)
        {
            if(timeoutTimeElapsed >= timeoutTime) {

                panel_pause.gameObject.SetActive(true);
                timeoutTimeElapsed = 0f;
            }
            else
            {
                timeoutTimeElapsed += Time.deltaTime;
            }
        }
        else if(timeoutTimeElapsed>0f)
        {
            timeoutTimeElapsed -= Time.deltaTime;

        }
    }
    private void OnFaceActive(bool active)
    {
        idle = !active;
    }
}
