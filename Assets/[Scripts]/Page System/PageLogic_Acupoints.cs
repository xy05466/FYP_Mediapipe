using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDM_FythmPipe
{
    public class PageLogic_Acupoints : PageLogic_Extension
    {
        public Acupoint accupoint;
        public List<AccupointStep> list_facepointSteps = new List<AccupointStep>();
        private List<Acupoint> list_accupoints = new List<Acupoint>();
        private int currentStep = -1;
        private int acupointDone = 0;
        private int maxAcupoint = 0;
        private bool finish = false;

        public override void Initialize()
        {
        }

        public override void StartPage()
        {
            acupointDone = 0;
            int maxPoints = 0;
            int currPoints = 0;
            foreach (var step in list_facepointSteps)
            {
                foreach (var index in step.indexes)
                    currPoints++;
                if (currPoints > maxPoints)
                    maxPoints = currPoints;
                currPoints = 0;
            }
            list_accupoints.Clear();
            maxAcupoint = maxPoints;

            // spawn points
            for (int i = 0; i < maxPoints; i++)
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
            if (!HasFacepointsLoaded || finish) return; // if facepoints loaded
                                                        //accupointManager.UpdateComponent();

            // set position of accupoints on facepoints
            for (int i = 0; i < GetCurrentStep().indexes.Count; i++)
            {
                // returns the point index
                int step_index = GetCurrentStep().indexes[i].GetLandmarkIndex();
                list_accupoints[i].transform.position = facepointsObj[step_index].transform.position;
            }
        }


        public override bool IsPageDone()
        {
            return finish;
        }

        public override void EndPage()
        {
        }

        public override void ResetPage()
        {
            Debug.Log("Reset page");
            currentStep = -1;
        }

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

        public override void OnHandAnnotationActive(int actives)
        {
        }

        public void NextStep()
        {
            acupointDone = 0;
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
            acupointDone++;

            if (acupointDone >= GetCurrentStep().indexes.Count)
            {
                NextStep();
            }
        }
        public AccupointStep GetCurrentStep()
        {
            return list_facepointSteps[currentStep];
        }

        public override int GetPageProgress()
        {
            return acupointDone;
        }

        public override int GetMaxPageProgress()
        {
            return maxAcupoint;
        }


        [System.Serializable]
        public class AccupointStep
        {
            public List<LandmarkData> indexes = new List<LandmarkData>();

            public void DebugIndexes()
            {
                string log = "[";

                foreach (var i in indexes)
                {
                    log += i + ",";
                }
                log += "]";
                Debug.Log(log);
            }
        }
    }
}