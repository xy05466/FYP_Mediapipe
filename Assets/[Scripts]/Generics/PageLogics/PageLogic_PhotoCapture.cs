using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_PhotoCapture : PageLogic_Extension
{
    public Button bu_done;
    public GameObject photoOverlay;
    public PhoneCamera phoneCamera;
    bool done = false;
    [Space]
    [SerializeField] TMP_Text textbox;
    [SerializeField] FacepointManager facepointManager;
    [SerializeField] Button capture;
    [SerializeField] float maxDistX = 3.0f;
    [SerializeField] float maxDistY = 3.0f;
    [SerializeField] float lowerBoundZ = 1.0f;
    [SerializeField] float upperBoundZ = 3.0f;
    [SerializeField] private JournalSO journalSO;


    public RawImage checkPic;
    public GameObject brightnessSprite;

    private float waitTime = 0f;
    private float timer = 0.0f;

    public GameObject audioManager;
    public Animator anim;

    float avgV = 0;
    private bool faceAligned;
        public override void Initialize()
    {
    }
    public override void StartPage()
    {
        bu_done.onClick.AddListener(()=> { done = true; });
        phoneCamera.StartPhoto();
        faceAligned = false;
        textbox.text = "No face detected!";
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        Vector3 refPt;
        float avgX = 0;
        float avgY = 0;
        float avgZ = 0;
        if (facepointsObj != null && facepointsObj.Count > 0)
        {

            refPt = facepointsObj[4].transform.position;
            avgX = refPt.x - Camera.main.transform.position.x;
            avgY = refPt.y - Camera.main.transform.position.y;
            avgZ = (refPt.z - Camera.main.transform.position.z) - 90.0f;
            if (avgZ < lowerBoundZ)
            {
                textbox.text = "Please move further";
                faceAligned = false;
            }
            else if (avgZ > upperBoundZ)
            {
                textbox.text = "Please move closer";
                faceAligned = false;
            }
            else if (avgX > maxDistX)
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

            if (timer > waitTime)
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

                Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
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


                    if (journalSO.GetHasCaptured())
                    {
                        textbox.text = "Perfect lighting! Take your 'after' picture. \n";
                    }
                    else
                    {
                        textbox.text = "Perfect lighting! Take your 'before' picture. \n";
                    }
                    capture.interactable = true;
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

                timer = timer - waitTime;

                Destroy(screenshotTexture);
                Destroy(renderTexture);
            }

        }
        else
        {
            capture.interactable = false;
        }
    }


    public override void EndPage()
    {
        bu_done.onClick.RemoveAllListeners();
        photoOverlay.gameObject.SetActive(false);
        done = false;
    }

    public override bool IsPageDone()
    {
        return done;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
    }

    public override void OnHandAnnotationActive(bool active)
    {
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
    }

    public override void ResetPage()
    {
    }


}
