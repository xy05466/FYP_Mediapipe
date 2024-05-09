using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHandsSceneManager : MonoBehaviour
{
    [SerializeField] FacepointManager fpm;
    [SerializeField] HandPointManager hpm;
    [SerializeField] List<GhostHandPathSO> leftHandPaths;
    [SerializeField] List<GhostHandPathSO> rightHandPaths;
    [SerializeField] GameObject handParent;

    GameObject leftHand;
    GameObject rightHand;

    GameObject[] handpoints;
    GameObject[] facepoints;

    int currentPathIndex;
    enum SceneState
    {
        STATE_START,
        STATE_INIT,
        STATE_PLAYPATHS,
    }
    SceneState sceneState;

    private void Awake()
    {
        sceneState = SceneState.STATE_START;
    }
    void Update()
    {
        handpoints = hpm.GetFacePointTransforms();
        facepoints = fpm.GetFacePointTransforms();
        if (handpoints != null)
        {
            if (handpoints.Length > 1)
            {

            }
        }
        if (facepoints != null)
        {
            if (facepoints.Length > 1) 
            {
                switch (sceneState)
                {
                    case SceneState.STATE_START:
                        leftHand = Instantiate(leftHandPaths[0].pathObject);
                        rightHand = Instantiate(rightHandPaths[0].pathObject);
                        SetPath(leftHandPaths[0], leftHand);
                        SetPath(rightHandPaths[0], rightHand);
                        leftHand.GetComponent<GhostHandMassageMovementController>().StartMoving();
                        rightHand.GetComponent<GhostHandMassageMovementController>().StartMoving();
                        sceneState = SceneState.STATE_INIT;
                        break;
                    case SceneState.STATE_INIT:
                        //Destroy(leftHand);
                        //Destroy(rightHand);
                        //currentPathIndex++;
                        //leftHand = Instantiate(leftHandPaths[currentPathIndex].pathObject);
                        //rightHand = Instantiate(rightHandPaths[currentPathIndex].pathObject);
                        break;
                    case SceneState.STATE_PLAYPATHS:

                        break;
                }
            } 
        }
    }
    void SetPath(GhostHandPathSO handPathSO, GameObject ghostHand)
    {
        List<GameObject> newPath = new List<GameObject>();
        foreach (int i in handPathSO.pathWaypoints)
        {
            newPath.Add(facepoints[i]);
        }
        ghostHand.GetComponent<GhostHandMassageMovementController>().SetMassagePath(newPath, 5);
    }
}
