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
    [SerializeField] float maxDistX = 3.0f;
    [SerializeField] float maxDistY= 3.0f;
    [SerializeField] float lowerBoundZ = 1.0f;
    [SerializeField] float upperBoundZ = 3.0f;
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
        Vector3 refPt = facePoints[4].transform.position;
        float avgX = 0;
        float avgY = 0;
        float avgZ = 0;
        avgX = refPt.x - Camera.main.transform.position.x;
        avgY = refPt.y - Camera.main.transform.position.y;
        avgZ = (refPt.z - Camera.main.transform.position.z) - 90.0f;
        textBoxString = "Face Distance: " + avgZ;
        //Debug.Log(avgZ);
        if (avgZ < lowerBoundZ)
        {
            textBoxString += "\nPlease move further";
        }
        else if (avgZ > upperBoundZ)
        {
            textBoxString += "\nPlease move closer";
        }
        else if (avgX > maxDistX)
        {
            textBoxString += "\nPlease move left";
        }
        else if (avgX < 0)
        {
            textBoxString += "\nPlease move right";
        }
        else if (avgY > maxDistY)
        {
            textBoxString += "\nPlease move down";
        }
        else if (avgY < 0)
        {
            textBoxString += "\nPlease move up";
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
