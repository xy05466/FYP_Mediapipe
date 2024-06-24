using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UIElements;

public class AccupointsMassageManagerFinal : MonoBehaviour
{

    [SerializeField] FacepointManager_Jaden _facepointManager;
    //[SerializeField] List<AccSOListList> accupointSOs;
    [SerializeField] List<AccupointCurveData> listSteps = new List<AccupointCurveData>();
    GameObject parent;
    //when init silder set 
    //public List<SilderSet> silderSetsList = new List<SliderSet>();
    public SilderSet sliderSet;
    [SerializeField] private Vector3 offset = Vector3.zero;
    private bool isAveraged = true;
    private bool lockedToZero = true;
    private int index = 0;
    [SerializeField] private GameObject InstructionName;
    private StepName InstructionNamestring;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject leftArrow;
    //time pressed
    public delegate void UpdateMassageUI(string value);
    // Declare a delegate variable
    public static event UpdateMassageUI onUpdateMassageUI;

    //list
    //how many sets in list
    //how to check if all sets are completed
    //spawning sets
    //new class
    private void Start()
    {
        if(InstructionName != null)
            InstructionNamestring = InstructionName.GetComponent<StepName>();

        index = 0;
        parent = this.gameObject;
        //silderSetsList = new List<GameObject>();
        //init all the sets
        //init points in slider
        //GameObject gameObject = Instantiate(silderPrefab, parent.transform);
        //gameObject.SetActive(true);
        //silderSetsList.Add(gameObject);
        //SilderSet currSilderSet = silderSetsList[index].GetComponent<SilderSet>();
        sliderSet.Init();
        onUpdateMassageUI((sliderSet.numOfSetsToComplete).ToString());
        SetUpPoints();
        sliderSet.OnComplete += SetDone;
    }
    List<Transform> positions;
    bool done = false;
    public bool IsDone()
    {
        return done;
    }
    void SetDone()
    {
        index++;
        sliderSet.ClearSet();
        if (index >= listSteps.Count)
        {
            done = true;
            return;
        }
        SetUpPoints();
        //onUpdateMassageUI((sliderSet.numOfSetsToComplete - index).ToString());
        if (sliderSet.numOfSetsToComplete - index <= 0)
        {
            if (rightArrow != null && leftArrow != null)
            {
                InstructionNamestring.stepNameString = "Right Jaw Glide";
                rightArrow.SetActive(true);
                leftArrow.SetActive(false);
                //onUpdateMassageUI((sliderSet.numOfSetsToComplete).ToString());
            }
        }
        else return;

    }
    void SetUpPoints()
    {
        for (int i = 0; i < listSteps[index].accupointSOs.Count; i++)
        {
            Debug.Log("currSilderSet.anchorPostions" + i);
            sliderSet.AddList(i);
        }
        sliderSet.SetUpPath();
    }
    Vector3 Reposition(List<Transform> positions, bool lockToZero, bool isAveraged = true)
    {
        if (positions != null)
        {
            float avgX = 0, avgY = 0, avgZ = 0;
            if (isAveraged)
            {
                foreach (Transform transform in positions)
                {
                    avgX += transform.position.x;
                    avgY += transform.position.y;
                    avgZ += transform.position.z;
                }
                avgX /= positions.Count;
                avgY /= positions.Count;
                avgZ /= positions.Count;
                avgX += offset.x;
                avgY += offset.y;
                avgZ += offset.z;

            }
            else
            {
                avgX = positions.Last().position.x;
                avgY = positions.Last().position.y;
                avgZ = positions.Last().position.z;
            }
            return new Vector3(avgX, avgY, avgZ);
        }
        else return Vector3.zero;
    }

    private void Update()
    {
        if (done) return;
        List<AccSOListList> steps = listSteps[index].accupointSOs;
        for (int i = 0; i < steps.Count; i++)
        {
            for (int j = 0; j < steps[i].accupointSOList.Count; j++)
            {
                int nextAccupoint = steps[i].accupointSOList[j].landmarkIndex;
                positions = _facepointManager.GetLandmarkPositionsByDigit(nextAccupoint);
                Vector3 position = Reposition(positions, lockedToZero, isAveraged);
                position = new Vector3(position.x, position.y, 0);
                sliderSet.anchorPostions.SetItemInList(i, j, position);
            }
        }
        //increase index when a set is done 
    }

}

