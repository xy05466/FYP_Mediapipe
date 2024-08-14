using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SDM_FythmPipe
{
    public abstract class PageLogic : MonoBehaviour
    {
        public PageData pageData;
        public ProgressionComponent progressionComponent;
        [Space(30)]
        protected PageState pageState = PageState.NOT_STARTED;
        private bool hasHandpointsLoaded = false;
        public bool HasHandpointsLoaded => hasHandpointsLoaded;
        private bool hasFacepointsLoaded = false;
        public bool HasFacepointsLoaded => hasFacepointsLoaded;

        public void LoadHandPoints(bool loaded)
        {
            hasHandpointsLoaded = loaded;
        }
        public void LoadFacePoints(bool loaded)
        {
            hasFacepointsLoaded = loaded;
        }
        // Will run at the start of scene
        public abstract void Initialize();
        // Execute when page has started
        public abstract void StartPage();
        // Execute when page is running
        public abstract void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj);

        // Check if page is done, move to next page is returns true
        public abstract bool IsPageDone();
        // Execute before going to next page
        public abstract void EndPage();
        // Execute when page reloaded
        public abstract void ResetPage();
        public abstract void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject);

        public virtual int GetPageProgress()
        {
            return 0;
        }

        public virtual int GetMaxPageProgress()
        {
            return 1;
        }

        // must be 0 to 1
        public virtual float GetSubProgression()
        {
            return 0;
        }

        // State comparison
        public void ChangeState(PageState pageState)
        {
            this.pageState = pageState;
        }
        public bool CompareState(PageState pageState)
        {
            return this.pageState == pageState;
        }
        public enum PageState
        {
            NOT_STARTED,
            START,
            ENDED
        }
    }
}