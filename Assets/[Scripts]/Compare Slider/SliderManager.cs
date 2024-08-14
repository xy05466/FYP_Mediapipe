using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderManager : MonoBehaviour
{
    public Slider slider;
    public GameObject sliderBG;
    public GameObject sliderFill;
    public TMP_Text dateText;
    public TMP_Text dayText;

    [SerializeField] private JournalSO journalSO;

    //Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            slider.GetComponent<RectTransform>().localScale = new Vector3(0.85f,0.85f,0.9f);
        }
    }

    public void LoadSliderImages(int i)
    {
        string fileLocation;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fileLocation = Application.persistentDataPath + "/AlbumPhoto/" + "Before" + "/" + i + ".png";
        }
        else
        {
            fileLocation = Application.dataPath + "/AlbumPhoto/" + "Before" + "/" + i + ".png";
        }
        FileStream beforeFs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        byte[] beforeImageData = new byte[beforeFs.Length];
        beforeFs.Read(beforeImageData, 0, (int)beforeFs.Length);
        Texture2D beforePic = new Texture2D(1080, 1920, TextureFormat.RGBA32, false);
        beforePic.LoadImage(beforeImageData);
        Sprite beforeSprite = Sprite.Create(beforePic, new Rect(0, 0, beforePic.width, beforePic.height), new Vector2(0.5f, 0.0f), 100.0f);

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fileLocation = Application.persistentDataPath + "/AlbumPhoto/" + "After" + "/" + i + ".png";
        }
        else
        {
            fileLocation = Application.dataPath + "/AlbumPhoto/" + "After" + "/" + i + ".png";

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
