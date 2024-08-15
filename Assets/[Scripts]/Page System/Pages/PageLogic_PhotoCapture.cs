using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class PageLogic_PhotoCapture : PageLogic
    {
        [Header("COMPONENTS")]
        //[SerializeField] FacepointManager facepointManager;
        public PhoneCamera phoneCamera;
        [Header("PROMPT")]
        [SerializeField] private GameObject photoOverlay;
        [SerializeField] private TMP_Text textbox;
        [SerializeField] private Button bu_capture;
        [SerializeField] private Button bu_retakePhoto;
        [SerializeField] private Button bu_done;
        [Header("ALIGNMENTMENT SETTINGS")]
        [SerializeField] private float maxDistX = 3.0f;
        [SerializeField] private float maxDistY = 3.0f;
        //[SerializeField] private float lowerBoundZ = 1.0f;
        //[SerializeField] private float upperBoundZ = 3.0f;
        //[SerializeField] private bool enableZ = true;
        public float waitTime = 0f;
        private float timer = 0.0f;
        [Header("JOURNAL DATA")]
        [SerializeField] private JournalData journalSO;

        public RawImage checkPic;
        public GameObject brightnessSprite;

        [Header("VISUAL FEEDBACK")]
        //public Image img_default;
        //public Image img_filled;
        public Animator anim;

        [Header("AUDIO FEEDBACK")]
        public GameObject audioManager;

        bool hasCaptureBefore = false;
        bool done = false;
        float avgV = 0;
        private bool faceAligned = false;
        public override void Initialize()
        {
            bu_capture.onClick.AddListener(CapturePhoto);
            bu_retakePhoto.onClick.AddListener(RetakePhoto);
            bu_done.onClick.AddListener(Continue);
        }
        private void Continue()
        {
            done = true;
            if (!hasCaptureBefore)
                hasCaptureBefore = true;
        }
        public void CapturePhoto()
        {
            bu_capture.interactable = false;
            photoOverlay.gameObject.SetActive(true);
            phoneCamera.StartSetPic();
        }
        public void RetakePhoto()
        {
            bu_capture.interactable = true;
            photoOverlay.gameObject.SetActive(false);
            if (!hasCaptureBefore)
                phoneCamera.SetHasCapturedBefore(false);
        }
        public override void StartPage()
        {
            photoOverlay.gameObject.SetActive(false);
            phoneCamera.StartPhoto();
            faceAligned = false;
            textbox.text = "No face detected!";
        }
        public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
        {
            Vector3 refPt;
            float avgX = 0;
            float avgY = 0;
            float avgZ = 0;
            if (facepointsObj != null && facepointsObj.Count > 0 && facepointsObj[0].activeInHierarchy)
            {

                refPt = facepointsObj[4].transform.position;
                avgX = refPt.x - Camera.main.transform.position.x;
                avgY = refPt.y - Camera.main.transform.position.y;
                avgZ = (refPt.z - Camera.main.transform.position.z) - 90.0f;
                //if (avgZ < lowerBoundZ && enableZ)
                //{
                //    textbox.text = "Please move further";
                //    faceAligned = false;
                //}
                //else if (avgZ > upperBoundZ && enableZ)
                //{
                //    textbox.text = "Please move closer";
                //    faceAligned = false;
                //}
                //else 
                if (avgX > maxDistX)
                {
                    textbox.text = "Please move left";
                    faceAligned = false;
                }
                else if (avgX < -1 * maxDistX)
                {
                    textbox.text = "Please move right";
                    faceAligned = false;
                }
                else if (avgY > maxDistY)
                {
                    textbox.text = "Please move down";
                    faceAligned = false;
                }
                else if (avgY < -1 * maxDistY)
                {
                    textbox.text = "Please move up";
                    faceAligned = false;
                }
                else
                {
                    faceAligned = true;
                }
            }
            else
            {
                textbox.text = "No face detected!!!!";
                faceAligned = false;
                //facePoints = facepointManager.GetFacePointTransforms();
            }

            if (faceAligned)
            {

                timer += Time.deltaTime;
                //if (img_filled != null)
                //{
                //    if (waitTime > 0)
                //    {
                //        img_filled.fillAmount = timer / waitTime;
                //        img_default.fillAmount = waitTime - timer;
                //    }
                //    else
                //    {
                //        img_filled.fillAmount = 1;
                //        img_default.fillAmount = 0;
                //    }
                //}

                if (timer >= waitTime)
                {

                    if (anim /*&& !brightnessSprite.GetComponent<Animator>().enabled*/)
                    {
                        //brightnessSprite.GetComponent<Animator>().enabled = true;
                        anim.enabled = true;
                    }

                    Debug.Log("DebugLightChecking");
                    Texture tex = checkPic.texture;

                    int width = tex.width;
                    int height = tex.height;

                    Texture2D screenshotTexture = new Texture2D((int)(width), (int)(height), TextureFormat.ARGB32, false);
                    RenderTexture currentRT = RenderTexture.active;

                    RenderTexture renderTexture = new RenderTexture(tex.width, tex.height, 32);
                    Graphics.Blit(tex, renderTexture);

                    RenderTexture.active = renderTexture;
                    screenshotTexture.ReadPixels(new UnityEngine.Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                    screenshotTexture.Apply();

                    UnityEngine.Color[] pixels = screenshotTexture.GetPixels();

                    RenderTexture.active = currentRT;

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        UnityEngine.Color.RGBToHSV(pixels[i], out float h, out float s, out float v);
                        avgV += v;
                    }

                    avgV /= tex.width * tex.height;

                    if (avgV > 0.35f)
                    {
                        if (anim)
                        {
                            anim.SetBool("isBlooming", true);
                            anim.SetBool("isClosing", false);
                        }


                        if (journalSO.GetHasCapturedBefore())
                        {
                            textbox.text = "Perfect lighting! Take your 'after' picture. \n";
                        }
                        else
                        {
                            textbox.text = "Perfect lighting! Take your 'before' picture. \n";
                        }
                        bu_capture.interactable = true;
                        Debug.Log("DebugPerfect");
                        if (!audioManager.GetComponent<AudioManager>().isPlaying("Sparkle"))
                        {
                            audioManager.GetComponent<AudioManager>().Play("Sparkle");
                        }
                    }
                    else if (avgV <= 0.35f)
                    {
                        if (anim)
                        {
                            anim.SetBool("isBlooming", false);
                            anim.SetBool("isClosing", true);
                        }


                        textbox.text = "Lighting is too dark. Please move to a brighter location. \n" + avgV.ToString();
                        Debug.Log("DebugDark");
                        if (!audioManager.GetComponent<AudioManager>().isPlaying("PlantsRustling"))
                        {
                            audioManager.GetComponent<AudioManager>().Play("PlantsRustling");
                        }
                    }

                    avgV = 0;

                    //timer = timer - waitTime;

                    Destroy(screenshotTexture);
                    Destroy(renderTexture);
                }
                else
                {
                    textbox.text = "Hold Still For " + waitTime.ToString("0") + " Seconds";
                }


            }
            else
            {
                timer -= Time.deltaTime;
                bu_capture.interactable = false;
            }
            timer = Mathf.Clamp(timer, 0, waitTime);
        }


        public override void EndPage()
        {
            done = false;
        }

        public override bool IsPageDone() => done;
    }
}