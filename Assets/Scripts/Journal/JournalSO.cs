using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[CreateAssetMenu]
public class JournalSO : ScriptableObject
{
  [field: SerializeField]
  public Texture2D dayOnePic { get; set; }

  [field: SerializeField]
  public Texture2D dayTenPic { get; set; }

  [field: SerializeField]
  public Texture2D dayFiftyPic { get; set; }

  [field: SerializeField]
  public Texture2D dayHundredPic { get; set; }

  [field: SerializeField]
  public string dayOneTxt { get; set; }

  [field: SerializeField]
  public string dayTenTxt { get; set; }

  [field: SerializeField]
  public string dayFiftyTxt { get; set; }

  [field: SerializeField]
  public string dayHundredTxt { get; set; }

  public IEnumerator TakePic(Texture pic)
  {
    yield return new WaitForEndOfFrame();

    if (!Directory.Exists(Application.persistentDataPath + "/Album"))
    {
      Directory.CreateDirectory(Application.persistentDataPath + "/Album");
    }

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

    //Rect rect = new Rect(0,0,width,height);
    //screenshotTexture.ReadPixels(rect, 0, 0);
    //screenshotTexture.Apply();

    int imageCount = PlayerPrefs.GetInt("imagecount");

    byte[] byteArray = screenshotTexture.EncodeToPNG();
    System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Album/CameraScreenshot" + imageCount + ".png", byteArray);
    Debug.Log(Application.dataPath);

   
  }

  public Texture GetPic(Texture pic, int n)
  {
    string fileLocation;
    dayOnePic = new Texture2D(pic.height,pic.width);
    int imageCount = PlayerPrefs.GetInt("imagecount");

    //if (!Directory.Exists(Path.GetDirectoryName(Application.persistentDataPath)))
    //{
    //  Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath));
    //}

    if (Application.platform == RuntimePlatform.Android)
    {

      fileLocation = Application.persistentDataPath + "/Album/CameraScreenshot" + n + ".png";
    }
    else
    {
      fileLocation = Application.dataPath + "/Album/CameraScreenshot" + n + ".png";
    }

    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

    byte[] imageData = new byte[fs.Length];
    fs.Read(imageData, 0, (int)fs.Length);
    dayOnePic.LoadImage(imageData);
    return (dayOnePic as Texture);


    //byte[] byteArray = File.ReadAllBytes(Application.dataPath + "/CameraScreenshot.png");
    //List<byte> byteList = new List<byte>(byteArray);


    //return dayOnePic;
  }
}
