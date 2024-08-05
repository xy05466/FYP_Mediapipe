using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class PageLogic : MonoBehaviour
{
    public PageData pageData;
    [Space(30)]
    public string leaveEmpty = "==============";
    protected PageState pageState = PageState.NOT_STARTED;
    private bool loadedHandpoints = false;
    public bool HandpointsLoaded => loadedHandpoints;
    private bool loadedFacepoints = false;
    public bool FacepointsLoaded => loadedFacepoints;

    public void LoadHandPoints(bool loaded)
    {
        loadedHandpoints = loaded;
    }
    public void LoadFacePoints(bool loaded)
    {
        loadedFacepoints = loaded;
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
    public abstract void OnHandPointCollideFacePoint(int hand,int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject);
    //protected bool CollideWithFacePoint(int pointIndex, int pointIndexCompare)
    //{
    //    return (pointIndex == pointIndexCompare);
    //}
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
