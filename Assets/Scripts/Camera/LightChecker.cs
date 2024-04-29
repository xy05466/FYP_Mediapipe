using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LightChecker : MonoBehaviour
{
    public RawImage checkPic;
    public TMP_Text brightnessTxt;
    public GameObject brightnessSprite;
    private float waitTime = 2.0f;
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            Texture tex = checkPic.texture;

            int width = tex.width;
            int height = tex.height;

            Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            RenderTexture currentRT = RenderTexture.active;

            RenderTexture renderTexture = new RenderTexture(tex.width, tex.height, 32);
            Graphics.Blit(tex, renderTexture);

            RenderTexture.active = renderTexture;
            screenshotTexture.ReadPixels(new UnityEngine.Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            screenshotTexture.Apply();

            UnityEngine.Color[] pixels = screenshotTexture.GetPixels();

            RenderTexture.active = currentRT;

            int sourceMipLevel = 0;
            float avgV = 0;
            Color32[] pixelArray = screenshotTexture.GetPixels32(sourceMipLevel);

            for (int i = 0; i < pixels.Length; i++)
            {
                UnityEngine.Color.RGBToHSV(pixels[i], out float h, out float s, out float v);
                avgV += v;
            }

            avgV /= tex.width * tex.height;

            if (avgV > 0.45f)
            {
                brightnessTxt.text = "just right " + avgV.ToString();

                if (!brightnessSprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("RoseBloom"))
                {
                    brightnessSprite.GetComponent<Animator>().Play("RoseBloom");
                }
                
                
                if (!this.GetComponent<AudioManager>().IsPlaying("Sparkle"))
                {
                    this.GetComponent<AudioManager>().Stop("PlantsRustling");
                    this.GetComponent<AudioManager>().Play("Sparkle");
                }
            }
            else
            {
                brightnessTxt.text = "too dark " + avgV.ToString();

                if (!brightnessSprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("RoseClose"))
                {
                    brightnessSprite.GetComponent<Animator>().Play("RoseClose");
                }
                if (!this.GetComponent<AudioManager>().IsPlaying("PlantsRustling"))
                {
                    this.GetComponent<AudioManager>().Stop("Sparkle");
                    this.GetComponent<AudioManager>().Play("PlantsRustling");
                }
            }

            avgV = 0;

            timer = timer - waitTime;

            Destroy(screenshotTexture);
            Destroy(renderTexture);
        }
    }
}
