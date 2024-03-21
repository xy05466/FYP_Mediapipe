using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class JournalManager : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;
    [SerializeField] private List<RawImage> pics;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera camera0;
    [SerializeField] private Camera camera1;

    public RawImage picResult;
    public TMP_InputField journalEntry;
    public TMP_Text journalDayTxt;

    private void Start()
    {
        //journalSO.Initialize();
        ShowPic();
    }

    public void ShowPic()
    {
        //PlayerPrefs.SetInt("imageCount",0);
        int picNum = journalSO.GetPicsCount();
        picResult.texture = journalSO.GetPicCache();
    }

    public void SetText()
    {
        int jNum = journalSO.GetPicsCount();
        journalSO.SetText(journalEntry.text, jNum);
    }

    public void StartConfirmPic(Camera cam)
    {
        StartCoroutine(ConfirmPic(cam));
    }

    public IEnumerator ConfirmPic(Camera cam)
    {
        yield return new WaitForEndOfFrame();
        //canvas.worldCamera = camera1;

        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 32);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();

        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.width);
        renderedTexture.ReadPixels(new Rect(0,0, Screen.width, Screen.width), (int)picResult.rectTransform.rect.x, (int)picResult.rectTransform.rect.y);
        RenderTexture.active = null;

        byte[] byteArray = renderedTexture.EncodeToPNG();
        string fileLocation;
        int i = journalSO.GetPicsCount();

        journalSO.SetDayNum(i);

        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/EditedAlbum"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/EditedAlbum");
            }
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/EditedAlbum"))
            {
                Directory.CreateDirectory(Application.dataPath + "/EditedAlbum");
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/EditedAlbum/CameraScreenshot" + i + ".png", byteArray);
        }
        else
        {
            System.IO.File.WriteAllBytes(Application.dataPath + "/EditedAlbum/CameraScreenshot" + i + ".png", byteArray);
        }

        canvas.worldCamera = camera0;
        //System.IO.File.WriteAllBytes(Application.dataPath + "/cameracapture.png", byteArray);
    }
  
    //public void SetDayOnePic()
    //{
    //    //picTaken = (Texture2D)background.texture;
    //    StartCoroutine(journalSO.TakePic(background.texture));
    //    imageCount++;
    //    PlayerPrefs.SetInt("imageCount", imageCount);
    //}

}
