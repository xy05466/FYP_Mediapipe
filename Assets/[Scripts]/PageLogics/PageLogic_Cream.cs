using System.Collections.Generic;
using UnityEngine;

public class PageLogic_Cream : PageLogic_Extension
{
    //[SerializeField] private List<GameObject> Wallpapers = new List<GameObject>();
    public List<FlowerAcupoint> allPetals = new List<FlowerAcupoint>();

    [SerializeField] private GameObject imageSegOBJ;
    private ImageSeg imageSeg;
    public FaceLandmarkObjectPlacerManager objectPlacer;
    int petalCount = 0;
    int petalDone = 0;
    bool done = false;

    public override void Initialize()
    {
        objectPlacer.PlaceObjects();
        for (int i = 0; i < objectPlacer.GetFacePointOjects().Count; i++)
        {
            allPetals.Add(objectPlacer.GetFacePointObject(i).GetComponent<FlowerAcupoint>());
            allPetals[i].pointId = objectPlacer.GetFacePointObject(i).GetIndex();
        }

        petalCount = allPetals.Count;

        foreach (FlowerAcupoint fc in allPetals) 
            fc.Subscribe(OnPetalDone);
    }
    private void OnPetalDone(FlowerAcupoint accupoint)
    {
        petalDone ++;
        if(petalDone>= petalCount)
        {
            done = true;
        }
    }
    public override void StartPage()
    {
        done = false;
        petalDone = 0;
        foreach (FlowerAcupoint fc in allPetals)
            fc.ResetPetal();

            if (imageSegOBJ)
            imageSeg = imageSegOBJ.GetComponent<ImageSeg>();
    }

    public override void ResetPage()
    {
    }

    public override void EndPage()
    {
    }

    public override bool IsPageDone()
    {
        return done;
    }

    public override void OnFaceAnnotationActive(bool active)
    {
    }

    public override void OnHandAnnotationActive(int actives)
    {

    }

    public override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject)
    {
        if (!gameObject.activeInHierarchy) return;
        foreach (var petal in allPetals)
        {
            if (petal.pointId == facePointIndex)
                petal.Fill();
        }
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
    {
    }

    public override int GetPageProgress()
    {
        return petalCount;
    }
    public override int GetMaxPageProgress()
    {
        return petalDone;
    }
}
