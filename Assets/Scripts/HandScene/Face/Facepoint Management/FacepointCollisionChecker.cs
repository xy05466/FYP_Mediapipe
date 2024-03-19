using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacepointCollisionChecker : MonoBehaviour
{
    [SerializeField] FacepointManager _facepointManager;
    List<int> landmarksToCheck;

    private void Awake()
    {
        landmarksToCheck = new List<int>();
        for (int i = 0; i < 468; ++i)
        {
            landmarksToCheck.Add(i);
        }
    }

    private void Update()
    {
        foreach (int landmark in landmarksToCheck)
        {
            if (_facepointManager.GetFacePoint(landmark).GetComponent<FacePointGO>().collided)
            {
                //Debug.Log("Facepoint " + landmark + " is colliding!");
            }
        }
    }
}
