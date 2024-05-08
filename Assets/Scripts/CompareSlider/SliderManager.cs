using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderManager : MonoBehaviour
{
    public GameObject sliderBG;
    public GameObject sliderFill;
    public TMP_Text dateText;
    public TMP_Text dayText;

    [SerializeField] private JournalSO journalSO;

    // Start is called before the first frame update
    //void Start()
    //{
    //    int imageCount = journalSO.GetBeforeCount();

    //    string fileLocation;
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        fileLocation = Application.persistentDataPath + "/Album/CameraScreenshot" + 1 + ".png";
    //    }
    //    else
    //    {
    //        fileLocation = Application.dataPath + "/Album/CameraScreenshot" + 1 + ".png";
    //    }
    //    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

    //    byte[] imageData = new byte[fs.Length];
    //    fs.Read(imageData, 0, (int)fs.Length);
    //    Texture2D pic = new Texture2D(1920, 1080, TextureFormat.RGBA32, false);
    //    pic.LoadImage(imageData);
    //    Sprite sprite = Sprite.Create(pic, new Rect(0, 0, pic.width, pic.height), new Vector2(0.5f, 0.0f), 100.0f);

    //    sliderBG.GetComponent<Image>().sprite = sprite;
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void LoadSliderImages(int i)
    {
        string fileLocation;
        if (Application.platform == RuntimePlatform.Android)
        {
            fileLocation = Application.persistentDataPath + "/BeforeAlbum/CameraScreenshot" + i + ".png";
        }
        else
        {
            fileLocation = Application.dataPath + "/BeforeAlbum/CameraScreenshot" + i + ".png";
        }
        FileStream beforeFs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        byte[] beforeImageData = new byte[beforeFs.Length];
        beforeFs.Read(beforeImageData, 0, (int)beforeFs.Length);
        Texture2D beforePic = new Texture2D(1080, 1920, TextureFormat.RGBA32, false);
        beforePic.LoadImage(beforeImageData);
        Sprite beforeSprite = Sprite.Create(beforePic, new Rect(0, 0, beforePic.width, beforePic.height), new Vector2(0.5f, 0.0f), 100.0f);

        if (Application.platform == RuntimePlatform.Android)
        {
            fileLocation = Application.persistentDataPath + "/AfterAlbum/CameraScreenshot" + i + ".png";
        }
        else
        {
            fileLocation = Application.dataPath + "/AfterAlbum/CameraScreenshot" + i + ".png";
        }
        FileStream afterFs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        byte[] afterImageData = new byte[afterFs.Length];
        afterFs.Read(afterImageData, 0, (int)afterFs.Length);
        Texture2D afterPic = new Texture2D(1080, 1920, TextureFormat.RGBA32, false);
        afterPic.LoadImage(afterImageData);
        Sprite afterSprite = Sprite.Create(afterPic, new Rect(0, 0, afterPic.width, afterPic.height), new Vector2(0.5f, 0.0f), 100.0f);
        
        sliderFill.GetComponent<Image>().sprite = beforeSprite;
        sliderBG.GetComponent<Image>().sprite = afterSprite;

        dateText.text = journalSO.date[i - 1];
        dayText.text = journalSO.day[i - 1];
    }

}
