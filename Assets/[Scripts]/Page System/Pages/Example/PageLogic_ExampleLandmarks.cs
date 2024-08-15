using SDM_FythmPipe;
using System.Collections.Generic;
using UnityEngine;

public class PageLogic_ExampleLandmarks : PageLogic
{
    public FaceLandmarkObjectPlacerManager FaceLandmarkObjectPlacerManager;
    public override void StartPage()
    {
        FaceLandmarkObjectPlacerManager.PlaceObjects();
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
    {
        base.RunLogic(handpointsObj_1, handpointsObj_2, facepointsObj);
    }
}
