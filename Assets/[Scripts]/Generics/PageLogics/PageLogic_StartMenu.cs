using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLogic_StartMenu : PageLogic
{
    public GameObject panel;
    public GameObject page_1;
    public GameObject page_2;
    public Button bu_start;
    bool pageEnd = false;


    public override void Initialize()
    {
        bu_start.onClick.AddListener(() => { pageEnd = true; });
    }
    public override void StartPage()
    {
        page_1.gameObject.SetActive(true);
        page_2.gameObject.SetActive(false);
    }
    public override void EndPage()
    {
        panel.gameObject.SetActive(false);
    }

    public override bool IsPageDone()
    {
        return pageEnd;
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
    }

    public override void ResetPage()
    {
        panel.gameObject.SetActive(true);
        pageEnd = false;
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
    }

}
