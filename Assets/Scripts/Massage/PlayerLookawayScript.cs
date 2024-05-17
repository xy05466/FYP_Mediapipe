using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerLookawayScript : MonoBehaviour
{
    [SerializeField] FacepointManager_Jaden _facepointManager;
    //handle when player look away from the script
    //r stands for referenced
    [SerializeField]
    private GameObject rGameObject;
    [SerializeField]
    Vector3 MaxPos = Vector3.zero;
    [SerializeField]
    Vector3 MinPos = Vector3.zero;
    //text
    public TMP_Text textBoxString;
    // Update is called once per frame
    void Update()
    {
        if (!FaceCheck())
            rGameObject.SetActive(false);
        else if (!rGameObject.activeInHierarchy)
            rGameObject.SetActive(true);
    }
    public bool FaceCheck()
    {
        GameObject[] facePoints = _facepointManager.GetFacePointTransforms();
        if (facePoints != null)
        {
            Vector3 refPt = facePoints[4].transform.position;
            float avgX = refPt.x - Camera.main.transform.position.x;
            float avgY = refPt.y - Camera.main.transform.position.y;
            float avgZ = (refPt.z - Camera.main.transform.position.z) - 90.0f;
            //textBoxString = "Face Distance: " + avgZ;
            //textBoxString = "Face location: " + avgX + "\n" + avgY + "\n" + avgZ;
            //Debug.Log(avgZ);
            
            if (avgZ < MinPos.z)
            {
                textBoxString.text = "Please move further";
                return false;
            }
            else if (avgZ > MaxPos.z)
            {
                textBoxString.text = "Please move closer";
                return false;

            }
            else if (avgX < MinPos.x)
            {
                textBoxString.text = "Please move right";
                return false;

            }
            else if (avgX > MaxPos.x)
            {
                textBoxString.text = "Please move left";
                return false;

            }
            else if (avgY < MinPos.y)
            {
                textBoxString.text = "Please move up";
                return false;

            }
            else if (avgY > MaxPos.y)
            {
                textBoxString.text = "\nPlease move down";
                return false;

            }
            return true;
        }
        else
        {
            textBoxString.text = "No face detected!";
            return false;

        }
    }
}
