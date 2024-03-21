using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[CreateAssetMenu]
public class JournalSO : ScriptableObject
{
    [field: SerializeField]
    public Texture2D dayOnePic { get; set; }

    [SerializeField]
    private Texture2D picsTaken {  get; set; }

    [SerializeField]
    private List<Texture2D> dayPics { get; set; }

    [SerializeField]
    private List <int> dayNum { get; set; }

    [SerializeField]
    private List <string> dayTxts { get; set; }

    [SerializeField]
    private int picsCount { get; set; }

    public void Initialize()
    {
        //dayPics = null;
        //dayTxts = null;
        //dayNum = null;
        //picsCount = 0;

        //Debug.Log(dayPics.Count);
        //Debug.Log(dayPics.Last());
        //Debug.Log(dayTxts.Last());
        if (dayTxts == null)
        {
            dayTxts = new List<string>();
            Debug.Log("newlist");
        }
        //if (dayPics != null)
        //    return;
        dayPics = new List<Texture2D>();
        string fileLocation;
        if (picsCount == 0)
        {
            return;
        }
        for (int i = 1; i < picsCount + 1; i++)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                fileLocation = Application.persistentDataPath + "/EditedAlbum/CameraScreenshot" + i + ".png";
            }
            else
            {
                fileLocation = Application.dataPath + "/EditedAlbum/CameraScreenshot" + i + ".png";
            }
            FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

            byte[] imageData = new byte[fs.Length];
            fs.Read(imageData, 0, (int)fs.Length);
            Texture2D pic = new Texture2D(351, 211);
            pic.LoadImage(imageData);
            dayPics.Add(pic);
        }

        //Debug.Log(dayPics.Last());
        //Debug.Log(dayTxts.Last());
    }

    public IEnumerator TakePic(Texture pic)
    {
    yield return new WaitForEndOfFrame();


    int width = pic.width;
    int height = pic.height;

    Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
    RenderTexture currentRT = RenderTexture.active;

    RenderTexture renderTexture = new RenderTexture(pic.width, pic.height, 32);
    Graphics.Blit(pic, renderTexture);

    RenderTexture.active = renderTexture;
    screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    screenshotTexture.Apply();

    Color[] pixels = screenshotTexture.GetPixels();

    RenderTexture.active = currentRT;

        picsTaken = screenshotTexture;

        //PlayerPrefs.SetInt("imageCount", imageCount++);

    //byte[] byteArray = screenshotTexture.EncodeToPNG();
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Album/CameraScreenshot" + (picsCount + 1) + ".png", byteArray);
    //    }
    //    else
    //    {
    //        System.IO.File.WriteAllBytes(Application.dataPath + "/Album/CameraScreenshot" + (picsCount + 1) + ".png", byteArray);
    //    }
    //    //Debug.Log(imageCount);
    //    Texture2D picTaken= new Texture2D(pic.width, pic.height);
    //    picTaken.LoadImage(byteArray);
    //    dayPics.Add(picTaken);
    //    picsCount = dayPics.Count;
    //    Debug.Log(dayPics.Count);
        //Debug.Log(dayPics.Last());

    }

    public void ConfirmPic()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/Album"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Album");
            }
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/Album"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Album");
            }
        }

        byte[] byteArray = picsTaken.EncodeToPNG();
        if (Application.platform == RuntimePlatform.Android)
        {
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Album/CameraScreenshot" + (picsCount + 1) + ".png", byteArray);
        }
        else
        {
            System.IO.File.WriteAllBytes(Application.dataPath + "/Album/CameraScreenshot" + (picsCount + 1) + ".png", byteArray);
        }
        //Debug.Log(imageCount);
        Texture2D picTaken = new Texture2D(picsTaken.width, picsTaken.height);
        picTaken.LoadImage(byteArray);
        dayPics.Add(picTaken);
        picsCount = dayPics.Count;
        //picsTaken = null;
        Debug.Log(dayPics.Count);
    }

    public Texture GetPicCache()
    {
        return picsTaken;
    }

    public Texture GetPicAlbum(int n)
    {
        //Debug.Log("getpic");
        //string fileLocation;
        //dayOnePic = new Texture2D(pic.height,pic.width);
        //int imageCount = PlayerPrefs.GetInt("imageCount");

        //if (Application.platform == RuntimePlatform.Android)
        //{

        //    fileLocation = Application.persistentDataPath + "/Album/CameraScreenshot" + n + ".png";
        //}
        //else
        //{
        //    fileLocation = Application.dataPath + "/Album/CameraScreenshot" + n + ".png";
        //}

        //FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        //byte[] imageData = new byte[fs.Length];
        //fs.Read(imageData, 0, (int)fs.Length);
        //dayOnePic.LoadImage(imageData);
        //return (dayOnePic as Texture);

        Debug.Log(dayPics.Count);
        
        return (dayPics[n]);
    }

    public void SetText(string txt, int n)
    {
        dayTxts.Add(txt);
    }

    public void SetDayNum(int n)
    {
        dayNum.Add(n);
    }

    public int GetDayNum(int n)
    {
        return dayNum[n];
    }

    public string GetText(int n)
    {
        return dayTxts[n];
    }

    public int GetPicsCount()
    {
        return picsCount;
    }
}
