using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mediapipe.Unity;
using Mediapipe;

public class HandPointManager : MonoBehaviour
{
    [SerializeField] MultiHandLandmarkListAnnotation _mhla;
    GameObject[] handPoints;
    public List<int> indexesToStore;
    private List<List<Transform>> landmarkPositions;
    private List<GameObject> faceCircles;
    [SerializeField] int threshold = 50;
    UnityEvent updateEvent;
    //Vector3 lastAverage;
    private void Awake()
    {
        // init all lists
        landmarkPositions = new List<List<Transform>>();
        for (int i = 0; i < indexesToStore.Count; ++i)
        {
            landmarkPositions.Add(new List<Transform>() { gameObject.transform });
            
        }

    }
    Vector3 CalculateAverage(int index)
    {
        float avgX = 0, avgY = 0, avgZ = 0;
        foreach (Transform transform in landmarkPositions[index])
        {
            avgX += transform.position.x;
            avgY += transform.position.y;
            avgZ += transform.position.z;
        }
        avgX /= landmarkPositions.Count;
        avgY /= landmarkPositions.Count;
        avgZ /= landmarkPositions.Count;
        return new Vector3(avgX, avgY, avgZ);
    }
    public Vector3 CalculateAverage(List<Transform> transforms)
    {
        float avgX = 0, avgY = 0, avgZ = 0;
        foreach (Transform transform in transforms)
        {
            avgX += transform.position.x;
            avgY += transform.position.y;
            avgZ += transform.position.z;
        }
        avgX /= landmarkPositions.Count;
        avgY /= landmarkPositions.Count;
        avgZ /= landmarkPositions.Count;
        return new Vector3(avgX, avgY, avgZ);
    }
    Transform checkObject;
    public void UpdateListData()
    {
        //Debug.Log(landmarkPositions.Count);
        if (_mhla != null && _mhla.isActiveInHierarchy && _mhla.transform.childCount > 0)
        {
            checkObject = _mhla.transform.GetChild(0);
            // get all 468 face points
            if (checkObject != null)
            {
                // TODO (Paul): clean up this warcrime
                //handPoints = _mhla.gameObject.transform.GetChild(0)?.GetChild(0)?.GetChild(0)?.GetComponentsInChildren<Transform>();
                handPoints = GameObject.FindGameObjectsWithTag("Handpoint");
                //update each respective list by searching indexesToStore
                //eg: landmarkPositions[i] = handPoints[n] where i is the index of the number n in indexes to store
                if (handPoints != null && handPoints.Length > 0)
                {
                    for (int i = 0; i < indexesToStore.Count; ++i)
                    {
                        //Debug.Log(landmarkPositions[i].Count);
                        int currentInt = indexesToStore[i];
                        if (Vector3.Distance(handPoints[currentInt].transform.position, CalculateAverage(i)) < threshold || landmarkPositions[i].Count < 10)
                        {
                            landmarkPositions[i].Add(handPoints[currentInt].transform);
                            if (landmarkPositions[i].Count > 10)
                                landmarkPositions[i].RemoveAt(0);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Returns the transform of a given facepoint
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetFacePoint(int index)
    {
        if (handPoints != null)
            return handPoints[index];
        else
            return null;
    }
    /// <summary>
    /// Returns the list of transforms for landmark n where n = digit
    /// Eg: passing 100 into the parameters returns landmark 100's positions in the last 10 frames
    /// If digit is not stored in indexesToStore (i.e not added into the editor) this function returns null
    /// </summary>
    /// <param name="digit">The number of the landmark whose positions you want</param>
    /// <returns></returns>
    public List<Transform> GetLandmarkPositionsByDigit(int digit)
    {
        if (digit > 468)
            return null;
        for (int counter = 0; counter < indexesToStore.Count; counter++)
        {
            if (digit == indexesToStore[counter])
            {
                return landmarkPositions[counter];
            }
        }
        return null;
    }
    /// <summary>
    /// Returns the list of transforms for landmark where n = 
    /// </summary>
    /// <param name="digit"></param>
    /// <returns></returns>
    public List<Transform> GetLandmarkPositionsByIndex(int digit)
    {
        return landmarkPositions[digit];
    }
    
    public GameObject[] GetFacePointTransforms()
    {
        return handPoints;
    }
    private void Update()
    {
        UpdateListData();
    }
}
