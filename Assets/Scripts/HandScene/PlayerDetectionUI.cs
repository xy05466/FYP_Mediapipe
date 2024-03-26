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
        avgZ = (facePoints[4].transform.position.z - Camera.main.transform.position.z) - 90.0f;
        textbox.text = "Face Distance: " + avgZ;
        //Debug.Log(avgZ);
    }
}
