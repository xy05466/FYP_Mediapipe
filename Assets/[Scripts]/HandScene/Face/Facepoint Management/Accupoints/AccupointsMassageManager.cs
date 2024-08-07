using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AccupointsMassageManager : MonoBehaviour
{
    [SerializeField] FacepointManager_Jaden _facepointManager;
    [SerializeField] List<AccSOListList>  accupointSOs;
    [SerializeField] GameObject accupointPrefab;
    GameObject parent;
    ListOfLists<GameObject> accupointsListLists;
    GameObject instance;
    [SerializeField] private Vector3 offset = Vector3.zero;
    private bool isAveraged = true;
    private bool lockedToZero = true;
    private int index = 0;

    int lastActivated, justActivated;
    private void Start()
    {
        index = 0;
        lastActivated = 0;
        justActivated = 0;
        parent = gameObject;
        AccupointMassage.onActivate += CheckOrder;
        accupointsListLists = new ListOfLists<GameObject>();
        //init all the different sets
        for (int i = 0; i < accupointSOs.Count(); i++)
        {
            accupointsListLists.AddList(new List<GameObject>());
            for (int j = 0; j < accupointSOs[i].accupointSOList.Count; j++)
            {
                instance = Instantiate(accupointPrefab, parent.transform);
                instance.GetComponent<AccupointMassage>().SetData(accupointSOs[i].accupointSOList[j]);
                instance.GetComponent<AccupointMassage>().SetOrder(j);
                //Debug.Log(instance.GetComponent<AccupointMassage>().GetOrder());
                instance.SetActive(false);
                accupointsListLists.AddItemToList(i, instance);
            }
        }
        foreach (GameObject item in accupointsListLists.GetList(index))
        {
            item.SetActive(true);
        }
    }
    List<Transform> positions;
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

        foreach (GameObject obj in accupointsListLists.GetList(index))
        {
            if (obj.activeInHierarchy)
            {
                //Debug.Log(obj.transform.position.x + ", " + obj.transform.position.y + ", " + obj.transform.position.z + " 1");
                int nextAccupoint = obj.GetComponent<AccupointMassage>().GetData().landmarkIndex;
                positions = _facepointManager.GetLandmarkPositionsByDigit(nextAccupoint);
                obj.transform.position = Reposition(positions, lockedToZero, isAveraged);
                if (lockedToZero)
                    obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
                //do order check here
                
                //When set finished go next
                if (obj.GetComponent<AccupointMassage>().GetOrder() == accupointsListLists.GetList(index).Count - 1)
                {
                    CheckCompleted();
                    if (obj.GetComponent<AccupointMassage>().GetActive())
                        if (index == accupointsListLists.CountOfLists() - 1)
                            Debug.Log("you are done yippee");
                        else
                            NextSet();
                }
            }
        }
    }
    private void NextSet()
    {
        lastActivated = 0;
        justActivated = 0;
        foreach (GameObject item in accupointsListLists.GetList(index))
        {
            item.SetActive(false);
        }
        index++;
        //deactivate previous set 
        //activate curr set
        foreach(GameObject item in accupointsListLists.GetList(index))
        {
            item.SetActive(true);
        }
    }
    private void ResetSet()
    {
        foreach (GameObject item in accupointsListLists.GetList(index))
        {
            item.GetComponent<AccupointMassage>().reset();
            lastActivated = 0;
            justActivated = 0;
        }
    }
    private void CheckOrder(int order)
    {
        lastActivated = justActivated;
        justActivated = order;
          
        if (lastActivated == justActivated - 1)
            return;
        else
            ResetSet();
    }
    private void CheckCompleted()
    {
        foreach (GameObject item in accupointsListLists.GetList(index))
        {
            if (item.GetComponent<AccupointMassage>().GetActive())
                return;

            ResetSet();
        }
    }
}


