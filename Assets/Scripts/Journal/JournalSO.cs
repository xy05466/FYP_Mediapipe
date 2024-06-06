using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[CreateAssetMenu]
public class JournalSO : ScriptableObject
{
    //[SerializeField]
    //public Texture2D picToLoad;

    [SerializeField]
    public Texture2D picsTaken {  get; set; }

    //[SerializeField]
    //public List<Texture2D> beforeList { get; set; }

    //[SerializeField]
    //public List<Texture2D> afterList { get; set; }

    [SerializeField]
    public List <int> dayNum { get; set; }

    [SerializeField]
    public List <string> dayTxts { get; set; }

    [SerializeField]
    public int beforeCount;

    [SerializeField]
    public int afterCount;

    [SerializeField]
    public bool hasCaptured;
    [SerializeField]
    public List<string> date;
    [SerializeField]
    public List<string> day;

    private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;

    public void Initialize()
    {
        //beforeList = null;
        //afterList = null;
        //dayTxts = null;
        //dayNum = null;

        //beforeCount = 0;
        //afterCount = 0;
        //hasCaptured = false;
        //date.Clear();
        //day.Clear();

        //Debug.Log(dayPics.Count);
        //Debug.Log(dayPics.Last());
        //Debug.Log(dayTxts.Last());


        if (dayTxts == null)
        {
            dayTxts = new List<string>();
        }
        //if (beforeList == null)
        //{
        //    beforeList = new List<Texture2D>();
        //}
        //if (afterList == null)
        //{
        //    afterList = new List<Texture2D>();
        //}
        if (dayNum == null)
        {
            dayNum = new List<int>();
        }
        //picToLoad = new Texture2D(2, 2);

        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    if (File.Exists(Application.persistentDataPath + "/BeforeAlbum/CameraScreenshot" + (beforeCount) + ".png"))
        //    {
        //        hasCaptured = true;
        //        Debug.Log("DebugsethascapturedTRUE");
        //        Debug.Log("Debug" + beforeCount.ToString());
        //    }
        //    else
        //    {
        //        hasCaptured = false;
        //        Debug.Log("DebugsethascapturedFALSE");
        //        Debug.Log("Debug" + beforeCount.ToString());
        //    }
        //}
        //else
        //{
        //    if (File.Exists(Application.dataPath + "/BeforeAlbum/CameraScreenshot" + (beforeCount) + ".png"))
        //    {
        //        hasCaptured = true;
        //    }
        //    else
        //    {
        //        hasCaptured = false;
        //    }
        //}

        //if (dayPics != null)
        //    return;

        if (beforeCount == 0)
        {
            return;
        }

        //string fileLocation;
        //for (int i = 1; i <= beforeCount; i++)
        //{
        //    if (Application.platform == RuntimePlatform.Android)
        //    {
        //        fileLocation = Application.persistentDataPath + "/BeforeAlbum/CameraScreenshot" + i + ".png";
        //    }
        //    else
        //    {
        //        fileLocation = Application.dataPath + "/BeforeAlbum/CameraScreenshot" + i + ".png";
        //    }
        //    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        //    byte[] imageData = new byte[fs.Length];
        //    fs.Read(imageData, 0, (int)fs.Length);
        //    Texture2D pic = new Texture2D(351, 211);
        //    pic.LoadImage(imageData);
        //    beforeList[beforeCount - 1] = pic;
        //}

        //for (int i = 1; i <= afterCount; i++)
        //{
        //    if (Application.platform == RuntimePlatform.Android)
        //    {
        //        fileLocation = Application.persistentDataPath + "/AfterAlbum/CameraScreenshot" + i + ".png";
        //    }
        //    else
        //    {
        //        fileLocation = Application.dataPath + "/AfterAlbum/CameraScreenshot" + i + ".png";
        //    }
        //    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        //    byte[] imageData = new byte[fs.Length];
        //    fs.Read(imageData, 0, (int)fs.Length);
        //    Texture2D pic = new Texture2D(351, 211);
        //    pic.LoadImage(imageData);
        //    afterList[afterCount - 1] = pic;
        //}

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

    RenderTexture renderTexture = new RenderTexture(width, height, 32);
    Graphics.Blit(pic, renderTexture);

    RenderTexture.active = renderTexture;

    screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    screenshotTexture.Apply();

    //Color[] pixels = screenshotTexture.GetPixels();

    RenderTexture.active = currentRT;

        picsTaken = screenshotTexture;
        FlipTextureVertically(picsTaken);
        //Debug.Log("Debug" +screenshotTexture.width + screenshotTexture.height);
        //Debug.Log("Debug" + picsTaken.width + picsTaken.height);

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

        //avgV /= picsTaken.width * picsTaken.height;

        //Debug.Log("Debug" + avgV.ToString());

        //avgV = 0;

        //Destroy(screenshotTexture);
        Destroy(renderTexture);
    }

    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    public static void FlipTextureVertically(Texture2D original)
    {
        var originalPixels = original.GetPixels();

        var newPixels = new Color[originalPixels.Length];

        var width = original.width;
        var rows = original.height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
            }
        }

        original.SetPixels(newPixels);
        original.Apply();
    }
    void CreateBaseDirectory()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/AlbumPhoto"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/AlbumPhoto");
            }
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/AlbumPhoto"))
            {
                Directory.CreateDirectory(Application.dataPath + "/AlbumPhoto");
            }
        }
    }
    public void ConfirmBeforePic(DateTime dateTime)
    {
        string dateTimeString = dateTime.ToString("dd_MM_yyyy");
        CreateBaseDirectory();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/AlbumPhoto/Before"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/AlbumPhoto/Before");
            }
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/AlbumPhoto/Before"))
            {
                Directory.CreateDirectory(Application.dataPath + "/AlbumPhoto/Before");
            }
        }

        DirectoryInfo info;
        byte[] byteArray;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            FlipTextureVertically(picsTaken);
            byteArray = rotateTexture(picsTaken, true).EncodeToPNG();

        }
        else
        {
            byteArray = rotateTexture(picsTaken, false).EncodeToPNG();
        }
        //Debug.Log("Debug" + picsTaken.width + picsTaken.height);
        
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/AlbumPhoto/Before/" + dateTimeString + ".png", byteArray);

            info = new DirectoryInfo(Application.persistentDataPath + "/AlbumPhoto/Before/");
        }
        else
        {
            System.IO.File.WriteAllBytes(Application.dataPath + "/AlbumPhoto/Before/" + dateTimeString + ".png", byteArray);
            info = new DirectoryInfo(Application.dataPath + "/AlbumPhoto/Before/");
        }

        beforeCount = info.GetFiles("*.png").Length;
        beforeCount = beforeCount++;

      
        //Debug.Log("Debug" + beforeCount);
        //picsTaken = null;
        //Debug.Log(picsCount + 1);
    }
    
    public void ConfirmAfterPic(DateTime dateTime)
    {
        string dateTimeString = dateTime.ToString("dd_MM_yyyy");

        CreateBaseDirectory();

        Debug.Log("confirmafterpic");
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/AlbumPhoto/After"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/AlbumPhoto/After");
            }
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/AlbumPhoto/After"))
            {
                Directory.CreateDirectory(Application.dataPath + "/AlbumPhoto/After");
            }
        }

        DirectoryInfo info;
        byte[] byteArray;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            FlipTextureVertically(picsTaken);
            byteArray = rotateTexture(picsTaken, true).EncodeToPNG();
        }
        else
        {
            byteArray = rotateTexture(picsTaken, false).EncodeToPNG();
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/AlbumPhoto/After/" + dateTimeString + ".png", byteArray);

            info = new DirectoryInfo(Application.persistentDataPath + "/AlbumPhoto/After/");
        }
        else
        {
            System.IO.File.WriteAllBytes(Application.dataPath + "/AlbumPhoto/After/" + dateTimeString + ".png", byteArray);
            info = new DirectoryInfo(Application.dataPath + "/AlbumPhoto/After/");
        }

        date.Add(DateTime.Today.ToString("dd MMMM yyyy"));
        day.Add(DateTime.Now.DayOfWeek.ToString());

        afterCount = info.GetFiles("*.png").Length;
        afterCount = afterCount++;
        //Debug.Log("Debug" + afterCount);

        //picsTaken = null;
        //Debug.Log(picsCount + 1);
    }

    public Texture GetPicCache()
    {
        return picsTaken;
    }
    //public Texture GetPhotoAlbum(AlbumPhotoType albumPhotoType, DateTime date)
    //{

    //    string albumType = "";
    //    switch (albumPhotoType)
    //    {
    //        case AlbumPhotoType.BEFORE:
    //            albumType = "before";
    //            break;
    //        case AlbumPhotoType.AFTER:
    //            albumType = "after";
    //            break;
    //        default:
    //            break;
    //    }
    //    string fileLocation;
    //    Debug.Log(date.Date.ToString());
    //    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //    {

    //        fileLocation = Application.persistentDataPath + "/AlbumPhoto/" + albumType + "/" + date.Date.ToString() + ".png";
    //    }
    //    else
    //    {
    //        fileLocation = Application.dataPath + "/AlbumPhoto/" + albumType + "/" + date.Date.ToString() + ".png";
    //    }

    //    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

    //    byte[] imageData = new byte[fs.Length];
    //    fs.Read(imageData, 0, (int)fs.Length);
    //    picToLoad.LoadImage(imageData);


    //    return (picToLoad);
    //}
    public enum AlbumPhotoType
    {
        BEFORE,
        AFTER
    }
    public Texture GetPhotoAlbum(AlbumPhotoType albumPhotoType, DateTime dateTime)
    {
        string dateTimeString = dateTime.ToString("dd_MM_yyyy");
        string albumType = "";

        switch (albumPhotoType)
        {
            case AlbumPhotoType.BEFORE:
                albumType = "Before";
                break;
            case AlbumPhotoType.AFTER:
                albumType = "After";
                break;
            default:
                break;
        }


        string path = "/AlbumPhoto/" + albumType.ToString() + "/" + dateTimeString + ".png";
        string fileLocation;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fileLocation = Application.persistentDataPath + path;
        }
        else
        {
            fileLocation = Application.dataPath + path;
        }
        Debug.Log(dateTimeString);
        Debug.Log(albumType.ToString());
        Debug.Log(path);

        FileStream fs;

        try
        {
            fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        }
        catch (Exception)
        {

            return (null);
        }


        byte[] imageData = new byte[fs.Length];
        fs.Read(imageData, 0, (int)fs.Length);
        Texture2D texture = new Texture2D(2,2);
        texture.LoadImage(imageData);
        return (texture);
    }
    //public Texture GetBeforeAlbum(int n)
    //{
    //    string fileLocation;
    //    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //    {

    //        fileLocation = Application.persistentDataPath + "/BeforeAlbum/CameraScreenshot" + n + ".png";
    //    }
    //    else
    //    {
    //        fileLocation = Application.dataPath + "/BeforeAlbum/CameraScreenshot" + n + ".png";
    //    }

    //    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

    //    byte[] imageData = new byte[fs.Length];
    //    fs.Read(imageData, 0, (int)fs.Length);
    //    picToLoad.LoadImage(imageData);

        
        
    //    return (picToLoad);
    //}

    //public Texture GetAfterAlbum(int n)
    //{
    //    string fileLocation;
    //    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //    {

    //        fileLocation = Application.persistentDataPath + "/AfterAlbum/CameraScreenshot" + n + ".png";
    //    }
    //    else
    //    {
    //        fileLocation = Application.dataPath + "/AfterAlbum/CameraScreenshot" + n + ".png";
    //    }

    //    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

    //    picToLoad = new Texture2D(2,2);

    //    byte[] imageData = new byte[fs.Length];
    //    fs.Read(imageData, 0, (int)fs.Length);
    //    picToLoad.LoadImage(imageData);


    //    return (picToLoad);
    //}    

    public void SetText(string txt, int n)
    {
        dayTxts.Add(txt);
    }

    public void SetDayNum(int n)
    {
        dayNum.Add(n);
    }

    //public void SetDayPics(Texture2D picTaken)
    //{
    //    beforeList.Add(picTaken);
    //}

    public void SetBeforeCount(int count)
    {
        beforeCount = count;
    }

    public void SetAfterCount(int count)
    {
        afterCount = count;
    }

    public int GetDayNum(int n)
    {
        return dayNum[n];
    }

    public string GetText(int n)
    {
        return dayTxts[n];
    }

    public int GetBeforeCount()
    {
        return beforeCount;
    }

    public int GetAfterCount()
    {
        return afterCount;
    }

    public void SetHasCaptured(bool b)
    {
        hasCaptured = b;
    }

    public bool GetHasCaptured()
    {
        return hasCaptured;
    }

    public string GetDate(int i)
    {
        return date[i];
    }
}
