using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class PageLogic_Cream : PageLogic
    {
        public List<FlowerAcupoint> allPetals = new List<FlowerAcupoint>();
        public FaceLandmarkObjectPlacerManager objectPlacer;
        int petalCount = 0;
        int petalDone = 0;
        bool creamDone = false;

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
            petalDone++;
            if (petalDone >= petalCount)
            {
                creamDone = true;
            }
        }
        public override void StartPage()
        {
            creamDone = false;
            petalDone = 0;
            foreach (FlowerAcupoint fc in allPetals)
                fc.ResetPetal();
        }

        public override bool IsPageDone() => creamDone;

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

        public override int GetPageProgress() => petalDone;
        public override int GetMaxPageProgress() => petalCount;
    }
}