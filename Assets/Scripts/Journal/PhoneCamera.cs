using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe;
using Mediapipe.Unity;
using TMPro;
using UnityEngine.Rendering;

public class PhoneCamera : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;
    [SerializeField] private List<RawImage> pics;

    private Texture2D picTaken;
    private bool camAvailable;
    private WebCamTexture backCam;
    private WebCamTexture camera;

    private List<WebCamTexture> cameras;
    private Texture defaultBackground;
    private string msg;
    private int imageCount;

    public TMP_Text text;
    public RawImage background;
    public RawImage picResult;
    public AspectRatioFitter fit;

    private void Start()
    {
        journalSO.Initialize();

        cameras = new List<WebCamTexture>();
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        //PlayerPrefs.SetInt("imageCount", 0);
        imageCount = journalSO.GetPicsCount();

        
        

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
        float ratio = (float)backCam.width * (1 / (float)backCam.height);
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;

        background.transform.localPosition = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

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
        StartCoroutine(SetPic());
    }

    IEnumerator SetPic()
    {
        //picTaken = (Texture2D)background.texture;
        yield return StartCoroutine(journalSO.TakePic(background.texture));
        ShowPic();
    }


    public void ConfirmPic()
    {
        journalSO.ConfirmPic();
    }


    private void OnDestroy()
    {
        backCam.Stop();
    }

    public void ShowPic()
    {

        imageCount = journalSO.GetPicsCount();
        Debug.Log(imageCount);
        picResult.texture = journalSO.GetPicCache();

    }
}
