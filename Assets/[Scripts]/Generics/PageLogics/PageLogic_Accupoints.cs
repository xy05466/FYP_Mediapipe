using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Page Logics/Page Logic Accupoints",fileName ="PageLogic_Accupoints")]
public class PageLogic_Accupoints : PageLogic
{
    public AccupointManager accupointManager;
    public AccupointNew accupoint;
    private List<AccupointNew> list_accupoints = new List<AccupointNew>();
    public List<AccupointStep> list_facepoints = new List<AccupointStep>();
    private int currentStep = -1;
    int accupointDone = 0;
    public override void StartPage()
    {
        int maxPoints = 0;
        int currPoints = 0;

        foreach (var step in list_facepoints)
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
            acc.gameObject.SetActive(false);
            acc.Subscribe(FinishAccupoint);
            list_accupoints.Add(acc);
        }
        //accupointManager.StartComponent();
        //GetCurrentStep().DebugIndexes();
        NextStep();
    }
    void FinishAccupoint(AccupointNew accupointSO)
    {
        accupointSO.ClearEvents();
        accupointSO.gameObject.SetActive(false);
        accupointDone++;

        if (accupointDone >= GetCurrentStep().indexes.Count)
        {
            NextStep();
        }
    }
    public override void RunLogic(List<HandPointAnnotationObject> handpointsObj, List<GameObject> facepointsObj)
    {
        if (!loadedFacepoints) return; // if facepoints loaded
        //accupointManager.UpdateComponent();

        // set position of accupoints on facepoints
        for (int i = 0; i < GetCurrentStep().indexes.Count; i++)
        {
            // returns the point index
            int step_index = GetCurrentStep().indexes[i].GetLandmarkIndex();
            list_accupoints[i].transform.position = facepointsObj[step_index].transform.position;
        }
    }
    public void NextStep()
    {
        accupointDone = 0;
        currentStep++;
        // Disable all accupoints first
        //for (int i = 0; i < list_accupoints.Count; i++)
            //list_accupoints[i].gameObject.SetActive(false);

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

    public AccupointStep GetCurrentStep()
    {
        return list_facepoints[currentStep];
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

