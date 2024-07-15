using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Page Logics/Page Logic Accupoints",fileName ="PageLogic_Accupoints")]
public class PageLogic_Accupoints : PageLogic_Extension
{
    public AccupointManager accupointManager;
    public AccupointNew accupoint;
    private List<AccupointNew> list_accupoints = new List<AccupointNew>();
    public List<AccupointStep> list_facepointSteps = new List<AccupointStep>();
    private int currentStep = -1;
    int accupointDone = 0;
    bool finish = false;

    public override void Initialize()
    {
    }

    public override void StartPage()
    {
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
        // spawn points
        for (int i = 0; i < maxPoints; i++)
        {
            AccupointNew acc = Instantiate(accupoint, transform);
            acc.Subscribe(FinishAccupoint);
            acc.gameObject.SetActive(true);
            acc.gameObject.transform.position =  new Vector3(10000,0,10000);
            list_accupoints.Add(acc);
        }
        //accupointManager.StartComponent();
        //GetCurrentStep().DebugIndexes();
        NextStep();
    }

    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (!FacepointsLoaded || finish) return; // if facepoints loaded
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
        return false;
    }

    public override void EndPage()
    {
    }

    public override void ResetPage()
    {
        Debug.Log("Reset page");
        currentStep = -1;
    }

    public override void OnHandPointCollideFacePoint(int handPointIndex, GameObject handPointObject, int facePointIndex, GameObject facePointObject)
    {
        if (finish) return;

        foreach (var stepIndex in GetCurrentStep().indexes)
        {
            if (facePointIndex == stepIndex.GetLandmarkIndex())
            {
                //test.gameObject.transform.position = facePointObject.transform.position;
                for (int i = 0; i < list_accupoints.Count; i++)
                {
                    AccupointNew accupointNew = list_accupoints[i];
                    if (accupointNew.GetAccID() == facePointIndex)
                        accupointNew.Colliding();
                    
                }
            }
        }
    }


    public override void OnFaceAnnotationActive(bool active)
    {
        if (!FacepointsLoaded || !CompareState(PageState.START) || list_accupoints.Count <=0) return;
        for (int i = 0; i < GetCurrentStep().indexes.Count; i++)
            list_accupoints[i].gameObject.SetActive(active);
    }

    public override void OnHandAnnotationActive(bool active)
    {
    }

    public void NextStep()
    {
        accupointDone = 0;
        currentStep++;
        if(currentStep >= list_facepointSteps.Count)
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

    void FinishAccupoint(AccupointNew accupointnew)
    {
        //accupointnew.ClearEvents();
        accupointnew.SetID(-1);
        accupointnew.gameObject.SetActive(false);
        accupointDone++;

        if (accupointDone >= GetCurrentStep().indexes.Count)
        {
            NextStep();
        }
    }
    public AccupointStep GetCurrentStep()
    {
        return list_facepointSteps[currentStep];
    }



    [System.Serializable]
    public class AccupointStep
    {
        public List<AccupointSO> indexes = new List<AccupointSO>();

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

