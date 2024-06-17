using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Button bu_close;
    public float duration;
    public CanvasGroup canvasGroup;
    private Coroutine coroutine;
    private void Start()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        bu_close.onClick.AddListener(StartFadeOut);
    }

    public void StartFadeOut()
    {
        if (coroutine != null)
            return;

        coroutine = StartCoroutine(fadeOut());
    }

    IEnumerator fadeOut()
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
        if (canvasGroup) return;
        canvasGroup.alpha = 1;
        gameObject.SetActive(true);
    }
}
