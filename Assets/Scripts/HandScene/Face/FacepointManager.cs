using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity;

public class FacepointManager : MonoBehaviour
{
    [SerializeField] MultiFaceLandmarkListAnnotation _mfla;
    Transform[] facePoints;
    public List<int> indexesToStore;
    private List<List<Transform>> landmarkPositions;
    private List<GameObject> faceCircles;
    private void Awake()
    {
        // init all lists
        landmarkPositions = new List<List<Transform>>();
        for (int i = 0; i < indexesToStore.Count; ++i)
        {
            landmarkPositions[i] = new List<Transform>();
        }
    }
    public void UpdateListData()
    {
        // get all 468 face points
        facePoints = _mfla.gameObject.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Transform>();
        // update each respective list by searching indexesToStore
        // eg: landmarkPositions[i] = facePoints[n] where i is the index of the number n in indexes to store 
        for (int i = 0; i < indexesToStore.Count; ++i)
        {
            int currentInt = indexesToStore[i];
            landmarkPositions[i].RemoveAt(0);
            landmarkPositions[i].Add(facePoints[currentInt]);
        }
    }
}
