using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class PhoneCamera : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;
    //[SerializeField] protected Mediapipe.Unity.Screen screen;
    [Space]
    private Texture2D picTaken;
    private bool camAvailable;
    private WebCamTexture backCam;
    private WebCamTexture frontCam;

    private List<WebCamTexture> cameras;
    //private Texture defaultBackground;
    private string msg;

    public RawImage background; // background
    //public GameObject screenLayer;
    public RawImage preview; 
    [Header("UI")]
    public TMP_Text brightnessTxt;
    public TMP_Text text; // camera text
    public TMP_Text date;
    public TMP_Text splashTxt;
    public TMP_Text photoTxt;
    //public UnityEngine.UI.Button albumButton;
    //public UnityEngine.UI.Button creamButton;
    public GameObject audioManager;

    public void StartPhoto()
    {
        journalSO.Initialize();
        
        Debug.Log("DebugBefore" + journalSO.GetBeforeCount());
        Debug.Log("DebugAfter" + journalSO.GetAfterCount());
        Debug.Log("Debug" + journalSO.GetHasCaptured());

        cameras = new List<WebCamTexture>();
        Debug.Log("cameras Count" + cameras.Count);

        //defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log("devices Count" + devices.Length);


        if (devices.Length < 1)
        {
            Debug.LogError("No cameras available!");
            camAvailable = false;
            return;
        }

        backCam = new WebCamTexture(devices[0].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
        for (int i = 0; i < devices.Length; i++)
        {
            frontCam = new WebCamTexture(devices[i].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
            cameras.Add(frontCam);
            msg += cameras[i].name;
            if(text)
            text.text = msg;

            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
            }
        }

        // check if has captured // idk why use scriptable object
        if (journalSO.GetHasCaptured())
        {
            //albumButton.gameObject.SetActive(true);
            //creamButton.gameObject.SetActive(false);
            if (splashTxt)
                splashTxt.text = "Lets take a ‘After’ photo.\nThis is to track your progress and compare the difference before & after the massage.";
            
            if(photoTxt)
            photoTxt.text = "After Photo";
            Debug.Log("Album");
        }
        else
        {
            //albumButton.gameObject.SetActive(false);
            //creamButton.gameObject.SetActive(true);
            if(splashTxt)
                splashTxt.text = "Let’s take a ‘Before’ photo!\nThis is to track your progress and see improvements over time using the app";
            if(photoTxt)
            photoTxt.text = "Before Photo";
            Debug.Log("Cream");
        }

        if (cameras.Count > 1) 
        { 
            backCam = cameras[1]; 
        }
        Debug.Log("back cam " + backCam);

        //background.texture = backCam;
        
        //background.GetComponent<RectTransform>().sizeDelta = new Vector2(backCam.width, backCam.height);

        date.text = DateTime.Today.ToString("dd MMMM yyyy");

        camAvailable = true;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            preview.rectTransform.localEulerAngles = new Vector3(0, 180, -90);
        }
        else
        {
            preview.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    public void ChangeFrontCam()
    {
        text.text = cameras[1].name;
        backCam = cameras[1];
        //background.texture = backCam;
        backCam.Play();
    }

    public void ChangeBackCam()
    {
        text.text = cameras[0].name;
        backCam = cameras[0];
        //background.texture = backCam;
        backCam.Play();
    }

    public void StartSetPic()
    {
        audioManager.GetComponent<AudioManager>().Play("CamCapture");
        StartCoroutine(SetPic());
    }

    IEnumerator SetPic()
    {
        preview.GetComponent<RectTransform>().sizeDelta = new Vector2(background.GetComponent<RectTransform>().rect.width, background.GetComponent<RectTransform>().rect.height);
       
        yield return StartCoroutine(journalSO.TakePic(background.texture));
        Debug.Log("Debug" + background.texture.width +"x" +background.texture.height);
        ShowPic();
        audioManager.GetComponent<AudioManager>().Play("Ding");
    }


    public void ConfirmPic()
    {
        if (!journalSO.GetHasCaptured())
        {
            journalSO.ConfirmBeforePic(DateTime.Now);
            Debug.Log("DebugConfirmBeforePic");
            Debug.Log("DebugBefore" + journalSO.GetBeforeCount().ToString());
            Debug.Log("Debug" + journalSO.GetHasCaptured());
        }
        else if (journalSO.GetHasCaptured())
        {
            journalSO.ConfirmAfterPic(DateTime.Now);
            Debug.Log("DebugConfirmAfterPic");
            Debug.Log("DebugAfter" + journalSO.GetAfterCount().ToString());
            Debug.Log("Debug" + journalSO.GetHasCaptured());
        }
        
    }
    public void ShowPic()
    {
        preview.texture = journalSO.GetPicCache();

    }

    public void SetHasCaptured(bool b)
    {
        journalSO.SetHasCaptured(b);
        Debug.Log("sethascaptured" + b.ToString());
    }

    private void OnDestroy()
    {
        if(backCam)
        backCam.Stop();
        string dateTimeString = DateTime.Now.ToString("dd_MM_yyyy");
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (File.Exists(Application.persistentDataPath + "/AlbumPhoto/Before/" + dateTimeString + ".png"))
            {

                journalSO.SetHasCaptured(true);
                //Debug.Log("DebugsethascapturedTRUE");
                //Debug.Log("DebugBefore" + journalSO.GetBeforeCount().ToString());
                //Debug.Log("DebugAfter" + journalSO.GetAfterCount().ToString());
                //Debug.Log("Debug" + journalSO.GetHasCaptured());
            }
            else
            {
                journalSO.SetHasCaptured(false);
                //Debug.Log("DebugsethascapturedFALSE");
                //Debug.Log("DebugBefore" + journalSO.GetBeforeCount().ToString());
                //Debug.Log("DebugAfter" + journalSO.GetAfterCount().ToString());
                //Debug.Log("Debug" + journalSO.GetHasCaptured());
            }
        }
        else
        {
            if (File.Exists(Application.dataPath + "/AlbumPhoto/Before/" + dateTimeString + ".png"))
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
