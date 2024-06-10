using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Page Logics/Page Logic Essence",fileName ="PageLogic_Essence")]
public class PageLogic_Essence : PageLogic
{
    public override void StartPage()
    {
        Debug.Log("page started");
    }

    public override void RunLogic()
    {

    }

    public override bool IsPageDone()
    {
        return false;
    }

    public override void EndPage()
    {
    }

    public override void ResetPage()
    {
        Debug.Log("Reset page");
    }

    public override void OnHandPointCollideFacePoint(int handPoint, int facePoint)
    {
        throw new System.NotImplementedException();
    }
}
