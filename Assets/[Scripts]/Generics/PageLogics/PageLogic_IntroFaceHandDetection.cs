using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_IntroFaceHandDetection : PageLogic_Extension
{
    [Header("Instruction Panel")]
    public GameObject panle_instruction;
    public TMP_Text txt_instruction;
    public Button bu_closeInstruction;
    [Header("Detection Settings")]
    public float faceTimeDetection = 3f;
    public float handTimeDetection = 3f;

    public override void StartPage()
    {

    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
    {
    }
    public override void EndPage()
    {
    }

    public override bool IsPageDone()
    {
        return false;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
    }

    public override void OnHandAnnotationActive(int actives)
    {
    }

    public override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject)
    {
    }

    public override void ResetPage()
    {
    }

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }
}
