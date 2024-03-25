using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDetectionUI : MonoBehaviour
{
    [SerializeField] TMP_Text textbox;
    [SerializeField] FacepointManager facepointManager;
    GameObject[] facePoints;
    private void Start()
    {
        textbox.text = "No face detected!";
        
    }
    private void Update()
    {
        facePoints = facepointManager.GetFacePointTransforms();       
        float avgZ = 0;
        foreach (GameObject point in facePoints)
        {
            avgZ += point.transform.position.z;
        }
        avgZ /= facePoints.Length;
        textbox.text = "Face Distance: " + avgZ;
        Debug.Log(avgZ);
    }
}
