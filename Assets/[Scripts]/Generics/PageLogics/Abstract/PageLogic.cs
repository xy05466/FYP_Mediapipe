using System.Collections;
using System.Collections.Generic;
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
    public abstract void StartPage();
    public abstract void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj);

    public abstract bool IsPageDone();
    public abstract void EndPage();
    public abstract void ResetPage();
    public abstract void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject);
    protected bool CollideWithFacePoint(int pointIndex, int pointIndexCompare)
    {
        return (pointIndex == pointIndexCompare);
    }
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
