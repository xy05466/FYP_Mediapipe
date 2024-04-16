using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccupointManager : MonoBehaviour
{
    [SerializeField] FacepointManager _facepointManager;
    [SerializeField] List<AccupointSO> accupointSOs;
    [SerializeField] GameObject accupointPrefab;
    [SerializeField] GameObject parent;
    List<GameObject> accupoints;
    GameObject instance;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private bool isAveraged = true;
    [SerializeField] private bool lockedToZero = false;
    public int numAccupointsLeft;
    private void Start()
    {
        //accupointTimers = new List<float>();
        accupoints = new List<GameObject>();
        for (int i = 0; i < accupointSOs.Count; ++i)
        {
            //accupointTimers.Add(3.0f);
            instance = Instantiate(accupointPrefab, parent.transform);
            instance.GetComponent<Accupoint>().SetData(accupointSOs[i]);
            accupoints.Add(instance);
        }
        numAccupointsLeft = accupoints.Count;
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
            //if (!lockToZero)
            return new Vector3(avgX, avgY, avgZ);
            //else
            //    return new Vector3(avgX, avgY, avgZ - avgZ);
        }
        else return Vector3.zero;
    }
    Vector3 Reposition(List<Transform> positions)
    {
        if (positions != null)
        {
            float avgX = 0, avgY = 0, avgZ = 0;
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
            return new Vector3(avgX, avgY, avgZ);
        }
        else return Vector3.zero;
    }
    public void Increase()
    {
        offset+= new Vector3(1 * Time.deltaTime, 1 * Time.deltaTime, 1 * Time.deltaTime);
    }
    public void Decrease()
    {
        offset -= new Vector3(1 * Time.deltaTime, 1 * Time.deltaTime, 1 * Time.deltaTime);
    }
    private void Update()
    {
        int counter = 0;
        foreach (GameObject obj in accupoints)
        {
            if (obj.activeInHierarchy)
            {
                //Debug.Log(obj.transform.position.x + ", " + obj.transform.position.y + ", " + obj.transform.position.z + " 1");
                int nextAccupoint = obj.GetComponent<Accupoint>().GetData().landmarkIndex;
                positions = _facepointManager.GetLandmarkPositionsByDigit(nextAccupoint);
                obj.transform.position = Reposition(positions, lockedToZero, isAveraged);
                if (lockedToZero)
                    obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
                counter++;
            }
        }
        numAccupointsLeft = counter;
    }
}
