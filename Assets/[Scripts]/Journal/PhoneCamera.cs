using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SDM_FythmPipe
{
    public class PhoneCamera : MonoBehaviour
    {
        [SerializeField] private JournalData journalSO;
        [Space]
        //private Texture2D picTaken;
        //private bool camAvailable;
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
        public AudioManager audioManager;

        public void StartPhoto()
        {
            cameras = new List<WebCamTexture>();

            //defaultBackground = background.texture;
            WebCamDevice[] devices = WebCamTexture.devices;


            if (devices.Length < 1)
            {
                Debug.LogError("No cameras available!");
                //camAvailable = false;
                return;
            }

            backCam = new WebCamTexture(devices[0].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
            for (int i = 0; i < devices.Length; i++)
            {
                frontCam = new WebCamTexture(devices[i].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
                cameras.Add(frontCam);
                msg += cameras[i].name;
                text.text = msg;

                if (!devices[i].isFrontFacing)
                {
                    backCam = new WebCamTexture(devices[i].name, UnityEngine.Screen.width, UnityEngine.Screen.height);
                }
            }

            // check if has captured // idk why use scriptable object
            if (journalSO.GetHasCapturedBefore())
            {
                if (splashTxt)
                    splashTxt.text = "Lets take a ‘After’ photo.\nThis is to track your progress and compare the difference before & after the massage.";
                photoTxt.text = "After Photo";
            }
            else
            {
                if (splashTxt)
                    splashTxt.text = "Let’s take a ‘Before’ photo!\nThis is to track your progress and see improvements over time using the app";
                photoTxt.text = "Before Photo";
            }

            if (cameras.Count > 1)
            {
                backCam = cameras[1];
            }
            Debug.Log("back cam " + backCam);

            //background.texture = backCam;

            //background.GetComponent<RectTransform>().sizeDelta = new Vector2(backCam.width, backCam.height);

            date.text = DateTime.Today.ToString("dd MMMM yyyy");

            //camAvailable = true;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                preview.rectTransform.localEulerAngles = new Vector3(0, 180, -90);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                preview.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
            }
            else
            {
                preview.rectTransform.localEulerAngles = new Vector3(0, 0, -90);
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
            audioManager?.Play("CamCapture");
            StartCoroutine(IeSetPic());
        }

        IEnumerator IeSetPic()
        {
            preview.GetComponent<RectTransform>().sizeDelta = new Vector2(background.GetComponent<RectTransform>().rect.width * 0.5f, background.GetComponent<RectTransform>().rect.height * 0.5f);

            yield return StartCoroutine(journalSO.IeTakePic(background.texture));
            //Debug.Log("Debug" + background.texture.width +"x" +background.texture.height);
            ShowPic();
            audioManager?.Play("Ding");
        }

        public void ConfirmPic()
        {
            if (!journalSO.GetHasCapturedBefore())
            {
                journalSO.ConfirmPic(JournalData.AlbumPhotoType.BEFORE,DateTime.Now);
                SetHasCapturedBefore(true);
            }
            else if (journalSO.GetHasCapturedBefore())
            {
                journalSO.ConfirmPic(JournalData.AlbumPhotoType.AFTER,DateTime.Now);
            }

        }
        public void ShowPic()
        {
            preview.texture = journalSO.GetPicCache();

        }

        public void SetHasCapturedBefore(bool b)
        {
            journalSO.SetHasCapturedBefore(b);
        }

        private void OnDestroy()
        {
            if (backCam)
                backCam.Stop();
        }
    }
}