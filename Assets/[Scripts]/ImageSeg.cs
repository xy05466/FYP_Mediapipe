using Mediapipe.Unity;
using System.Net;
using UnityEngine;
using UnityEngine.Video;

public class ImageSeg : MaskAnnotation
{
    string relativePath = "";

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayWallpaper(int wallpaperIndex)
    {

        switch (wallpaperIndex)
        {
            case 1:
                maskRenderTexture.Create();
                relativePath = "Resources/LiveWallpaper/Sakura.mp4";
                break;
            case 2:
                maskRenderTexture.Create();
                relativePath = "Resources/LiveWallpaper/Roses.mp4";
                break;
            case 3:
                maskRenderTexture.Create();
                ClearRenderTexture();
                relativePath = "";
                break;
            default:
                Debug.LogError("Invalid wallpaper index.");
                return;
        }

        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, relativePath);
        if (relativePath == "")
        {
            filterTex.url = "";
        }
        else
        {
            filterTex.url = videoPath;
            filterTex.targetTexture = maskRenderTexture;
            filterTex.Prepare();
            filterTex.prepareCompleted += OnVideoPrepareCompleted;
        }
    }

    private void OnVideoPrepareCompleted(VideoPlayer source)
    {
        source.Play();

        source.prepareCompleted -= OnVideoPrepareCompleted;
    }

    private void ClearRenderTexture()
    {
        if (maskRenderTexture != null)
        {
            maskRenderTexture.Release();
        }
    }
}
