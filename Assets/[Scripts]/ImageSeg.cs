using Mediapipe.Unity;
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
                relativePath = "Resources/LiveWallpaper/Sakura.mp4";
                break;
            case 2:
                relativePath = "Resources/LiveWallpaper/Roses.mp4";
                break;
            case 3:
                relativePath = "Resources/LiveWallpaper/AbstractLines.mp4";
                break;
            default:
                Debug.LogError("Invalid wallpaper index.");
                return;
        }

        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, relativePath);
        filterTex.url = videoPath;
        filterTex.targetTexture = maskRenderTexture;
        filterTex.Prepare();

        // Add event listener to play the video once it's prepared
        filterTex.prepareCompleted += OnVideoPrepareCompleted;
    }

    private void OnVideoPrepareCompleted(VideoPlayer source)
    {
        // Play the video
        source.Play();

        // Remove the event listener to prevent it from being called multiple times
        source.prepareCompleted -= OnVideoPrepareCompleted;
    }
}
