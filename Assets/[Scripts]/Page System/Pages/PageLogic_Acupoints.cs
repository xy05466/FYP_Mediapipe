using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class PageLogic_Acupoints : PageLogic
    {
        public Acupoint accupoint;
        public List<AcupointStep> list_facepointSteps = new List<AcupointStep>();
        private List<Acupoint> list_accupoints = new List<Acupoint>();
        private int currentStep = -1;
        private int currentAcupointsDone = 0;
        private int maxAcupointsSpawned = 0;
        private bool finish = false;

        public override void StartPage()
        {
            currentStep = -1;
            currentAcupointsDone = 0;
            maxAcupointsSpawned = 0;
            int currPoints = 0;
            foreach (var step in list_facepointSteps)
            {
                foreach (var index in step.indexes)
                    currPoints++;
                if (currPoints > maxAcupointsSpawned)
                    maxAcupointsSpawned = currPoints;
                currPoints = 0;
            }
            list_accupoints.Clear();

            // spawn points
            for (int i = 0; i < maxAcupointsSpawned; i++)
            {
                Acupoint acc = Instantiate(accupoint, transform);
                acc.Subscribe(FinishAccupoint);
                acc.gameObject.SetActive(true);
                acc.gameObject.transform.position = new Vector3(10000, 0, 10000);
                list_accupoints.Add(acc);
            }
            //accupointManager.StartComponent();
            //GetCurrentStep().DebugIndexes();
            NextStep();
        }

        public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
        {
            if (!HasFacepointsLoaded || finish) return; 

            // set position of accupoints on facepoints
            for (int i = 0; i < GetCurrentStep().indexes.Count; i++)
            {
                // returns the point index
                int step_index = GetCurrentStep().indexes[i].GetLandmarkIndex();
                list_accupoints[i].transform.position = facepointsObj[step_index].transform.position;
            }
        }


        public override bool IsPageDone() => finish;

        public override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject)
        {
            if (finish) return;

            foreach (var stepIndex in GetCurrentStep().indexes)
            {
                if (facePointIndex == stepIndex.GetLandmarkIndex())
                {
                    // loop all acupoints
                    for (int i = 0; i < list_accupoints.Count; i++)
                    {
                        Acupoint accupointNew = list_accupoints[i];
                        if (accupointNew.GetAccID() == facePointIndex)
                            accupointNew.Colliding();
                    }
                }
            }
        }


        public override void OnFaceAnnotationActive(bool active)
        {
            if (!HasFacepointsLoaded || !CompareState(PageState.START) || list_accupoints.Count <= 0) return;
            for (int i = 0; i < GetCurrentStep().indexes.Count; i++)
                list_accupoints[i].gameObject.SetActive(active);
        }

        public void NextStep()
        {
            currentAcupointsDone = 0;
            currentStep++;
            if (currentStep >= list_facepointSteps.Count)
            {
                finish = true;
                return;
            }
            // set Id base on current steps
            for (int i = 0; i < GetCurrentStep().indexes.Count; i++)
            {
                //list_accupoints[i].gameObject.SetActive(true);
                // returns the point index
                int step_index = GetCurrentStep().indexes[i].GetLandmarkIndex();
                list_accupoints[i].AccupointReset();
                list_accupoints[i].gameObject.SetActive(true);
                list_accupoints[i].Initialize();
                list_accupoints[i].SetID(step_index);
            }

        }

        void FinishAccupoint(Acupoint accupointnew)
        {
            //accupointnew.ClearEvents();
            accupointnew.SetID(-1);
            accupointnew.gameObject.SetActive(false);
            currentAcupointsDone++;

            if (currentAcupointsDone >= GetCurrentStep().indexes.Count)
            {
                NextStep();
            }
        }
        public AcupointStep GetCurrentStep()=> list_facepointSteps[currentStep];

        public override int GetPageProgress() => currentStep;

        public override int GetMaxPageProgress() => list_facepointSteps.Count;


        [System.Serializable]
        public class AcupointStep
        {
            public List<LandmarkData> indexes = new List<LandmarkData>();
        }
    }
}