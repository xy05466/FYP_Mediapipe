using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageLogic : MonoBehaviour
{
    public bool loadedHandponts = false;
    public bool loadedFacepoints = false;
    public abstract void StartPage();
    public abstract void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj);

    public abstract bool IsPageDone();
    public abstract void EndPage();
    public abstract void ResetPage();
    public abstract void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject);


}
