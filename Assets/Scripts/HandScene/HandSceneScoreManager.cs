using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandSceneScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text textbox;
    [SerializeField] FacepointManager facepointManager;
    [SerializeField] AccupointManager accupointManager;
    [SerializeField] GameObject button; 
    [SerializeField] float lowerBound = 1.0f;
    [SerializeField] float upperBound = 3.0f;
    GameObject[] facePoints;
    bool canMoveToNextScene;
    private void Start()
    {
        canMoveToNextScene = true;
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
        if (avgZ < lowerBound)
        {
            textBoxString += "\nPlease move further";
        }
        else if (avgZ > upperBound)
        {
            textBoxString += "\nPlease move closer";
        }

        if (accupointManager.numAccupointsLeft > 0)
        {
            textBoxString += "\nNumber of accupoints left: " + accupointManager.numAccupointsLeft;
        }
        else
        {
            textBoxString += "Congratulations! You've activated all accupoints!";
            canMoveToNextScene = true;
        }
        textbox.text = textBoxString;
    }
}
