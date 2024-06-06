using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class AccupointsMassageManagerFinal : MonoBehaviour
{

    [SerializeField] FacepointManager_Jaden _facepointManager;
    [SerializeField] List<AccSOListList> accupointSOs;
    [SerializeField] GameObject silderPrefab;
    GameObject parent;
    //when init silder set 
    List<GameObject> silderSetsList;
    [SerializeField] private Vector3 offset = Vector3.zero;
    private bool isAveraged = true;
    private bool lockedToZero = true;
    private int index = 0;

    //list
    //how many sets in list
    //how to check if all sets are completed
    //spawning sets
    //new class
    private void Start()
    {
        index = 0;
        parent = this.gameObject;
        silderSetsList = new List<GameObject>();
        //init all the sets
        //init points in slider
        GameObject gameObject = Instantiate(silderPrefab, parent.transform);
        gameObject.SetActive(true);
        silderSetsList.Add(gameObject);
        SilderSet currSilderSet = silderSetsList[index].GetComponent<SilderSet>();
        currSilderSet.Init();



        for (int i = 0; i < accupointSOs.Count; i++)
        {
            Debug.Log("currSilderSet.anchorPostions" + i);

            currSilderSet.AddList(i);


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

        SilderSet currSilderSet = silderSetsList[index].GetComponent<SilderSet>();

        for (int i = 0; i < accupointSOs.Count; i++)
        {
            for (int j = 0; j < accupointSOs[i].accupointSOList.Count; j++)
            {
                int nextAccupoint = accupointSOs[i].accupointSOList[j].landmarkIndex;
                positions = _facepointManager.GetLandmarkPositionsByDigit(nextAccupoint);
                Vector3 position = Reposition(positions, lockedToZero, isAveraged);
                position = new Vector3(position.x, position.y, 0);
                currSilderSet.anchorPostions.SetItemInList(i, j, position);
            }
        }
        //increase index when a set is done 
    }

}

