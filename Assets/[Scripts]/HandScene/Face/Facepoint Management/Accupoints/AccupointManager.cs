using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccupointManager : MonoBehaviour
{
    [SerializeField] FacepointManager _facepointManager;
    [SerializeField] List<AccupointSO> accupointSOs;
    [SerializeField] List<Vector2> steps;
    [SerializeField] Accupoint accupointPrefab;
    [SerializeField] GameObject parent;
    List<Accupoint> accupoints;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private bool isAveraged = true;
    [SerializeField] private bool lockedToZero = false;
    public int numAccupointsLeft;
    int currentStep;
    bool nextX, nextY;
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
            //if (!lockToZero)
            return new Vector3(avgX, avgY, avgZ);
            //else
            //    return new Vector3(avgX, avgY, avgZ - avgZ);
        }
        else return Vector3.zero;
    }
    public void StartComponent()
    {       
        //accupointTimers = new List<float>();
        accupoints = new List<Accupoint>();
        for (int i = 0; i < accupointSOs.Count; ++i)
        {
            //accupointTimers.Add(3.0f);
            Accupoint instance = Instantiate(accupointPrefab, parent.transform);
            instance.GetComponent<Accupoint>().SetData(accupointSOs[i]);
            instance.gameObject.SetActive(false);
            accupoints.Add(instance);
        }
        numAccupointsLeft = accupoints.Count;
        currentStep = 0;
        nextX = false;
        nextY = false;
    }
    public void UpdateComponent()
    {
        _facepointManager.UpdateListData();
        if (steps.Count > 0)
        {
            if (steps[currentStep].x != -1 && !nextX)
            {
                Accupoint currentPoint = accupoints[(int)steps[currentStep].x];
                if (!currentPoint.gameObject.activeInHierarchy)
                {
                    if (currentPoint.GetComponent<Accupoint>().timer < 3)
                    {
                        Debug.Log("xtrue");
                        currentPoint.gameObject.SetActive(true);
                    }
                    else
                        nextX = true;
                }
            }
            else
            {
                nextX = true;
            }
            if (steps[currentStep].y != -1 && !nextY)
            {
                Accupoint currentPoint = accupoints[(int)steps[currentStep].y];
                if (!currentPoint.gameObject.activeInHierarchy)
                {
                    if (currentPoint.GetComponent<Accupoint>().timer < 3)
                    {
                        Debug.Log("ytrue");
                        currentPoint.gameObject.SetActive(true);
                    }
                    else
                        nextY = true;
                }
            }
            else
            {
                nextY = true;
            }
            //int counter = 0;
            foreach (Accupoint obj in accupoints)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    //Debug.Log(obj.transform.position.x + ", " + obj.transform.position.y + ", " + obj.transform.position.z + " 1");
                    int nextAccupoint = obj.GetComponent<Accupoint>().GetData().landmarkIndex;
                    positions = _facepointManager.GetLandmarkPositionsByDigit(nextAccupoint);
                    obj.transform.position = Reposition(positions, lockedToZero, isAveraged);
                    if (lockedToZero)
                        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
                    //counter++;
                }
            }
            if (nextX && nextY)
            {
                currentStep++;
                nextX = false;
                nextY = false;
            }
            numAccupointsLeft = currentStep; //counter;
        }
    }
}
