using System.Collections.Generic;
using UnityEngine;

public class AccupointManager : MonoBehaviour
{
    [SerializeField] FacepointManager _facepointManager;
    [SerializeField] List<AccupointSO> accupointSOs;
    [SerializeField] GameObject accupointPrefab;
    [SerializeField] GameObject parent;
    List<float> accupointTimers;
    List<GameObject> accupoints;
    GameObject instance;
    private void Start()
    {
        accupointTimers = new List<float>();
        accupoints = new List<GameObject>();
        for (int i = 0; i < accupointSOs.Count; ++i)
        {
            accupointTimers.Add(3.0f);
            instance = Instantiate(accupointPrefab, parent.transform);
            instance.GetComponent<Accupoint>().SetData(accupointSOs[i]);
            accupoints.Add(instance);
        }
    }
    List<Transform> positions;
    //private void Update()
    //{
    //    // TODO (Paul): change accupointSOs to use Int + Vec3
    //    positions = _facepointManager.GetLandmarkPositionsByIndex(/*(int)accupointSOs[0].pointTethering.w*/ 4);
    //    Debug.Log(positions.Count);
    //    if (positions != null)
    //    {
    //        float avgX = 0, avgY = 0, avgZ = 0;
    //        foreach (Transform transform in positions)
    //        {
    //            avgX += transform.position.x;
    //            avgY += transform.position.y;
    //            avgZ += transform.position.z;
    //        }
    //        avgX /= positions.Count;
    //        avgY /= positions.Count;
    //        avgZ /= positions.Count;
    //        instance.transform.position = new Vector3(avgX, avgY, avgZ);
    //        //instance.transform.position = _facepointManager.CalculateAverage(positions);
    //    }
    //}
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
            return new Vector3(avgX, avgY, avgZ);
        }
        else return Vector3.zero;
    }
    private void Update()
    {
        for(int i = 0; i < accupoints.Count; ++i)
        {
            GameObject obj = accupoints[i];
            bool isColliding = obj.GetComponent<FacePointGO>().collided;
            if (isColliding)
            {
                accupointTimers[i] -= Time.deltaTime;
                Debug.Log(accupointTimers[i].ToString());
            }
            if (accupointTimers[i] < 0)
            {
                GameObject gameObjToRemove = accupoints[i];
                accupointTimers.RemoveAt(i);
                accupoints.RemoveAt(i);
                Destroy(gameObjToRemove);
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (GameObject obj in accupoints)
        {
            int nextAccupoint = obj.GetComponent<Accupoint>().GetData().landmarkIndex;
            //Debug.Log(nextAccupoint);
            positions = _facepointManager.GetLandmarkPositionsByDigit(/*(int)accupointSOs[0].pointTethering.w*/nextAccupoint);
            //Debug.Log(positions.Count);
            obj.transform.position = Reposition(positions);
        }
    }
}
