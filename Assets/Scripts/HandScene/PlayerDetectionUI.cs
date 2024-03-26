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
    string textBoxString;
    private void Update()
    {
        facePoints = facepointManager.GetFacePointTransforms();       
        float avgZ = 0;
        avgZ = (facePoints[4].transform.position.z - Camera.main.transform.position.z) - 90.0f;
        textBoxString = "Face Distance: " + avgZ;
        //Debug.Log(avgZ);
        if (avgZ < 1.5f)
        {
            textBoxString += "\nPlease move further";
        }
        else if (avgZ > 2.5f)
        {
            textBoxString += "\nPlease move closer";
        }
        textbox.text = textBoxString;
    }
}
