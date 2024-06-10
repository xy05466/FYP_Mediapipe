using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageLogic : MonoBehaviour
{
    public abstract void StartPage();
    public abstract void RunLogic();

    public abstract bool IsPageDone();
    public abstract void EndPage();
    public abstract void ResetPage();
    public abstract void OnHandPointCollideFacePoint(int handPoint, int facePoint);


}
