using UnityEngine;

namespace SDM_FythmPipe
{
    [CreateAssetMenu(menuName = "Scriptable/Page Data", fileName = "PageData_")]
    public class PageData : ScriptableObject
    {
        public bool enableHeader = true;
        public bool autoNavigate = false;
        [Header("Splash Data")]
        public bool enableTransition = true;
        public bool enableSplashPage = true;
        // splash image
        public Sprite spr_splashIcon;
        // title
        public string title;
        public string titleSub;
        [TextArea]
        public string splashDescription;
        //[TextArea]
        //public string instructionDescription;

        [Header("Trackings")]
        // annotation to enable
        public bool enableFaceTracking;
        public bool enableHandTracking;

    }
}