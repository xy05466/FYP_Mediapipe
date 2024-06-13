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
    private AccupointsMassageManagerFinal rGameObject;
    [SerializeField]
    Vector3 MaxPos = Vector3.zero;
    [SerializeField]
    Vector3 MinPos = Vector3.zero;
    //text
    public TMP_Text textBoxString;
    public bool gameStart = false;
    private void Start()
    {

        rGameObject.StartFunc();


    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStart)
            return;
        if (!FaceCheck())
        {
            rGameObject.gameObject.SetActive(false);
            Debug.Log("not active");
        }
        else if (!rGameObject.gameObject.activeInHierarchy)
        {
            Debug.Log("active");
            rGameObject.gameObject.SetActive(true);
        }
        if(rGameObject.gameObject.activeInHierarchy)
            rGameObject.UpdateScene();

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
                if(textBoxString)
                    textBoxString.text = "Please move further";
                return false;
            }
            else if (avgZ > MaxPos.z)
            {
                if (textBoxString)
                    textBoxString.text = "Please move closer";
                return false;

            }
            if (avgX < MinPos.x)
            {
                if (textBoxString)
                    textBoxString.text = "Please move right";
                return false;

            }
            else if (avgX > MaxPos.x)
            {
                if (textBoxString)
                    textBoxString.text = "Please move left";
                return false;

            }
            if (avgY < MinPos.y)
            {
                if (textBoxString)
                    textBoxString.text = "Please move up";
                return false;

            }
            else if (avgY > MaxPos.y)
            {
                if (textBoxString)
                    textBoxString.text = "\nPlease move down";
                return false;

            }
            return true;
        }
        else
        {
            if (textBoxString)
                textBoxString.text = "No face detected!";
            return false;

        }
    }
}
