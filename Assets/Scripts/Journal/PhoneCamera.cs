using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PhoneCamera : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;
    [SerializeField] protected Mediapipe.Unity.Screen screen;

    private Texture2D picTaken;
    private bool camAvailable;
    private WebCamTexture backCam;
    private WebCamTexture camera;

    private List<WebCamTexture> cameras;
    private Texture defaultBackground;
    private string msg;

    public TMP_Text text;
    public RawImage background;
    public RawImage preview;
    public RawImage picResult;
    public TMP_Text brightnessTxt;
    public UnityEngine.UI.Button albumButton;
    public UnityEngine.UI.Button creamButton;
    public GameObject audioManager;

    private void Start()
    {
        journalSO.Initialize();
        
        Debug.Log("DebugBefore" + journalSO.GetBeforeCount());
        Debug.Log("DebugAfter" + journalSO.GetAfterCount());
        Debug.Log("Debug" + journalSO.GetHasCaptured());

        cameras = new List<WebCamTexture>();
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        //imageCount = journalSO.GetBeforeCount();

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
            albumButton.gameObject.SetActive(true);
            creamButton.gameObject.SetActive(false);
        }
        else
        {
            albumButton.gameObject.SetActive(false);
            creamButton.gameObject.SetActive(true);
        }

        backCam.Play();
        background.texture = backCam;
        
        background.GetComponent<RectTransform>().sizeDelta = new Vector2(backCam.width, backCam.height);
        preview.GetComponent<RectTransform>().sizeDelta = new Vector2(backCam.width, backCam.height);
        camAvailable = true;
    }
    private void Update()
    {
        if (!camAvailable)
        {
            return;
        }

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;

        background.transform.localPosition = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        picResult.rectTransform.localEulerAngles = new Vector3(0,0, orient);
    }

    //private void StartBrightnessCheck()
    //{
    //    StartCoroutine(BrightnessCheck());
    //}

    //private void BrightnessCheck()
    //{
    //    Graphics.ConvertTexture(background.texture, texture2D);

    //    for (int i = 0; i < texture2D.width; i++)
    //    {
    //        for (int j = 0; j < texture2D.height; j++)
    //        {
    //            UnityEngine.Color pixel = texture2D.GetPixel(i, j);
    //            UnityEngine.Color.RGBToHSV(pixel, out h, out s, out v);
    //            avgH += h;
    //            avgS += s;
    //            avgV += v;
    //        }
    //    }

    //    //avgH *= (texture2D.width / 100) + (texture2D.height / 100);
    //    //avgS *= (texture2D.width / 100) + (texture2D.height / 100);
    //    //avgV *= (texture2D.width / 100) + (texture2D.height / 100);

    //    brightnessTxt.text = avgH + ", " + avgS + ", " + avgV;
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
        audioManager.GetComponent<AudioManager>().Play("CamCapture");
        StartCoroutine(SetPic());
    }

    IEnumerator SetPic()
    {
        //picTaken = (Texture2D)background.texture;
        yield return StartCoroutine(journalSO.TakePic(background.texture));
        ShowPic();
        audioManager.GetComponent<AudioManager>().Play("Ding");
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
