using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe;
using Mediapipe.Unity;
using TMPro;
using UnityEngine.Rendering;
using System.IO;
using UnityEditor;
using static Unity.VisualScripting.Member;
using UnityEngine.UIElements;

public class PhoneCamera : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;

    private Texture2D picTaken;
    private bool camAvailable;
    private WebCamTexture backCam;
    private WebCamTexture camera;

    private List<WebCamTexture> cameras;
    private Texture defaultBackground;
    private string msg;
    private int imageCount;
    private Texture2D texture2D;
    private Texture2D checkPic;

    private float waitTime = 2.0f;
    private float timer = 0.0f;

    private int width, height;

    float r, g, b;
    float h, s, v;
    float avgG, avgR, avgB;
    float avgH, avgS, avgV;

    public TMP_Text text;
    public RawImage background;
    public RawImage picResult;
    public AspectRatioFitter fit;
    public TMP_Text brightnessTxt;
    public UnityEngine.UI.Button albumButton;
    public UnityEngine.UI.Button creamButton;
    public GameManager gameManager;

    private void Start()
    {
        journalSO.Initialize();

        Debug.Log("DebugBefore" + journalSO.GetBeforeCount());
        Debug.Log("DebugAfter" + journalSO.GetAfterCount());
        Debug.Log("Debug" + journalSO.GetHasCaptured());

        cameras = new List<WebCamTexture>();
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        //PlayerPrefs.SetInt("imageCount", 0);
        imageCount = journalSO.GetBeforeCount();

        if (devices.Length < 1)
        {
            Debug.LogError("No cameras available!");
            camAvailable = false;
            return;
        }

        backCam = new WebCamTexture(devices[0].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
        for (int i = 0; i < devices.Length; i++)
        {
            camera = new WebCamTexture(devices[i].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
            cameras.Add(camera);
            msg += cameras[i].name;
            text.text = msg;

            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
            }
        }

        if (journalSO.GetHasCaptured())
        {
            albumButton.enabled = true;
            creamButton.enabled = false;
        }
        else
        {
            albumButton.enabled = false;
            creamButton.enabled = true;
        }

        backCam.Play();
        background.texture = backCam;
        camAvailable = true;        
    }
    private void Update()
    {
        if (!camAvailable)
        {
            return;
        }
        //float ratio = (float)backCam.width * (1 / (float)backCam.height);
        //fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;

        background.transform.localPosition = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        picResult.rectTransform.localEulerAngles = new Vector3(0,0, orient);

        //timer += Time.deltaTime;

        //// Check if we have reached beyond 2 seconds.
        //// Subtracting two is more accurate over time than resetting to zero.
        //if (timer > waitTime)
        //{
        //    BrightnessCheck(background.texture);
        //    // Remove the recorded 2 seconds.
        //    timer = timer - waitTime;
        //}
    }

    //private void BrightnessCheck(Texture bg)
    //{
    //    int width = bg.width;
    //    int height = bg.height;

    //    Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
    //    RenderTexture currentRT = RenderTexture.active;

    //    RenderTexture renderTexture = new RenderTexture(width, height, 32);
    //    Graphics.Blit(bg, renderTexture);

    //    RenderTexture.active = renderTexture;
    //    screenshotTexture.ReadPixels(new UnityEngine.Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    //    screenshotTexture.Apply();

    //    UnityEngine.Color[] pixels = screenshotTexture.GetPixels();

    //    RenderTexture.active = currentRT;

    //    checkPic = screenshotTexture;

    //    int sourceMipLevel = 0;
    //    float avgV = 0;
    //    Color32[] pixelArray = checkPic.GetPixels32(sourceMipLevel);

    //    for (int i = 0; i < pixels.Length; i++)
    //    {
    //        UnityEngine.Color.RGBToHSV(pixels[i], out float h, out float s, out float v);
    //        avgV += v;
    //    }

    //    avgV /= checkPic.width * checkPic.height;

    //    if (avgV > 0.45f)
    //    {
    //        brightnessTxt.text = "just right " + avgV.ToString();
    //    }
    //    else
    //    {
    //        brightnessTxt.text = "too dark " + avgV.ToString();
    //    }

    //    avgV = 0;

    //    Destroy(screenshotTexture);
    //    Destroy(renderTexture);
    //}

    public void ChangeFrontCam()
    {
        text.text = cameras[1].name;
        backCam = cameras[1];
        background.texture = backCam;
        backCam.Play();
    }

    public void ChangeBackCam()
    {
        text.text = cameras[0].name;
        backCam = cameras[0];
        background.texture = backCam;
        backCam.Play();
    }

    public void StartSetPic()
    {
        gameManager.GetComponent<AudioManager>().Play("CamCapture");
        StartCoroutine(SetPic());
    }

    IEnumerator SetPic()
    {
        //picTaken = (Texture2D)background.texture;
        yield return StartCoroutine(journalSO.TakePic(background));
        //BrightnessCheck(background.texture);
        ShowPic();
        gameManager.GetComponent<AudioManager>().Play("Ding");
    }

    public void ConfirmPic()
    {
        if (!journalSO.GetHasCaptured())
        {
            journalSO.ConfirmBeforePic();
            Debug.Log("DebugConfirmBeforePic");
            Debug.Log("DebugBefore" + journalSO.GetBeforeCount().ToString());
            Debug.Log("Debug" + journalSO.GetHasCaptured());
        }
        else if (journalSO.GetHasCaptured())
        {
            journalSO.ConfirmAfterPic();
            Debug.Log("DebugConfirmAfterPic");
            Debug.Log("DebugAfter" + journalSO.GetAfterCount().ToString());
            Debug.Log("Debug" + journalSO.GetHasCaptured());
        }
    }
    public void ShowPic()
    {

        //imageCount = journalSO.GetBeforeCount();
        //Debug.Log(imageCount);
        picResult.texture = journalSO.GetPicCache();

    }

    public void SetHasCaptured(bool b)
    {
        journalSO.SetHasCaptured(b);
        Debug.Log("sethascaptured" + b.ToString());
    }

    private void OnDestroy()
    {
        backCam.Stop();

        if (Application.platform == RuntimePlatform.Android)
        {
            if (File.Exists(Application.persistentDataPath + "/BeforeAlbum/CameraScreenshot" + (journalSO.GetBeforeCount()) + ".png"))
            {
                journalSO.SetHasCaptured(true);
                Debug.Log("DebugsethascapturedTRUE");
                Debug.Log("DebugBefore" + journalSO.GetBeforeCount().ToString());
                Debug.Log("DebugAfter" + journalSO.GetAfterCount().ToString());
                Debug.Log("Debug" + journalSO.GetHasCaptured());
            }
            else
            {
                journalSO.SetHasCaptured(false);
                Debug.Log("DebugsethascapturedFALSE");
                Debug.Log("DebugBefore" + journalSO.GetBeforeCount().ToString());
                Debug.Log("DebugAfter" + journalSO.GetAfterCount().ToString());
                Debug.Log("Debug" + journalSO.GetHasCaptured());
            }
        }
        else
        {
            if (File.Exists(Application.dataPath + "/BeforeAlbum/CameraScreenshot" + (journalSO.GetBeforeCount()) + ".png"))
            {
                journalSO.SetHasCaptured(true);
            }
            else
            {
                journalSO.SetHasCaptured(false);
            }
        }
        
    }

    
}
