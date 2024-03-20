using System.Collections.Generic;
using UnityEngine;

public class AccupointManager : MonoBehaviour
{
    [SerializeField] FacepointManager _facepointManager;
    [SerializeField] List<AccupointSO> accupointSOs;
    [SerializeField] GameObject accupointPrefab;
    [SerializeField] GameObject parent;
    List<GameObject> accupoints;
    GameObject instance;
    private void Start()
    {
        accupoints = new List<GameObject>();
        for (int i = 0; i < accupointSOs.Count; ++i)
        {
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
    private void FixedUpdate()
    {
        foreach (GameObject obj in accupoints)
        {
            positions = _facepointManager.GetLandmarkPositionsByDigit(/*(int)accupointSOs[0].pointTethering.w*/obj.GetComponent<Accupoint>().GetData().landmarkIndex);
            obj.transform.position = Reposition(positions);
        }
    }
}
