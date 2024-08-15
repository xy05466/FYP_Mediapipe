using UnityEngine;
using UnityEngine.Video;

namespace SDM_FythmPipe
{
    public class VideoEventListener : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public RenderTexture renderTexture;

        public void PrepareAndPlay(string videoPath)
        {
            videoPlayer.url = videoPath;
            videoPlayer.targetTexture = renderTexture;
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += OnVideoPrepareCompleted;
        }

        private void OnVideoPrepareCompleted(VideoPlayer source)
        {
            source.Play();
            source.prepareCompleted -= OnVideoPrepareCompleted;
        }
    }
}
