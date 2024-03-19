using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mediapipe.Unity;
using Mediapipe;

public class FacepointManager : MonoBehaviour
{
    [SerializeField] MultiFaceLandmarkListAnnotation _mfla;
    Transform[] facePoints;
    public List<int> indexesToStore;
    private List<List<Transform>> landmarkPositions;
    private List<GameObject> faceCircles;
    UnityEvent updateEvent;
    private void Awake()
    {
        // init all lists
        landmarkPositions = new List<List<Transform>>();
        for (int i = 0; i < indexesToStore.Count; ++i)
        {
            landmarkPositions.Add(new List<Transform>());
        }
    }
    GameObject checkObject;
    public void UpdateListData()
    {
        //Debug.Log(landmarkPositions.Count);
        checkObject = _mfla.transform.GetChild(0).gameObject;
        // get all 468 face points
        if (checkObject.activeInHierarchy && checkObject != null)
        {
            facePoints = _mfla.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<Transform>();
            //update each respective list by searching indexesToStore
            //eg: landmarkPositions[i] = facePoints[n] where i is the index of the number n in indexes to store
            if (facePoints != null)
            {
                for (int i = 0; i < indexesToStore.Count; ++i)
                {
                    //Debug.Log(landmarkPositions[i].Count);
                    int currentInt = indexesToStore[i];
                    landmarkPositions[i].Add(facePoints[currentInt]);
                    if (landmarkPositions[i].Count > 10)
                        landmarkPositions[i].RemoveAt(0);
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
        return facePoints[index + 1].gameObject;
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
    
    public Transform[] GetFacePointTransforms()
    {
        return facePoints;
    }
    private void Update()
    {
        UpdateListData();
    }
}
