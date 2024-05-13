using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeSplashScreen : MonoBehaviour
{
    public float duration;
    Color newColor;
    Image[] images;
    TMP_Text[] texts;

    private void Start()
    {
        images= GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TMP_Text>();
    }

    public void StartFadeOut()
    {
        StartCoroutine(fadeOut());
    }

    IEnumerator fadeOut()
    {
        float counter = 0;
        //Get current color
        Color spriteColor = this.GetComponent<RawImage>().material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            Debug.Log(alpha);

            //Change alpha only
            this.GetComponent<RawImage>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            foreach (Image image in images)
            {
                newColor = image.color;
                newColor.a = alpha;
                image.color = newColor;
            }
            foreach (TMP_Text texts in texts)
            {
                newColor = texts.color;
                newColor.a = alpha;
                texts.color = newColor;
            }
            //Wait for a frame
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
