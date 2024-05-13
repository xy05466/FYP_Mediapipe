using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostHandsSceneManager : MonoBehaviour
{
    [SerializeField] FacepointManager fpm;
    [SerializeField] HandPointManager hpm;
    [SerializeField] List<GhostHandPathSO> leftHandPaths;
    [SerializeField] List<GhostHandPathSO> rightHandPaths;
    [SerializeField] GameObject handParent;

    public static UnityEvent FinishedPathEvent;

    GameObject leftHand;
    GameObject rightHand;

    GameObject[] handpoints;
    GameObject[] facepoints;

    int currentLeftPathIndex;
    int currentRightPathIndex;
    enum SceneState
    {
        STATE_START,
        STATE_NEXT,
        STATE_DONE,
    }
    SceneState sceneState;

    private void Awake()
    {
        sceneState = SceneState.STATE_START;
        FinishedPathEvent = new UnityEvent();
        FinishedPathEvent.AddListener(OnPathTraversed);
        currentLeftPathIndex = 0;
        currentRightPathIndex = 0;
    }
    void Update()
    {
        handpoints = hpm.GetFacePointTransforms();
        facepoints = fpm.GetFacePointTransforms();
        if (facepoints != null)
        {
            if (facepoints.Length > 1) 
            {
                switch (sceneState)
                {
                    case SceneState.STATE_START:
                        StartGenerateGhostHands();
                        sceneState = SceneState.STATE_NEXT;
                        break;
                    case SceneState.STATE_NEXT:
                        break;
                    case SceneState.STATE_DONE:
                        Destroy(leftHand);
                        Destroy(rightHand);
                        if (currentLeftPathIndex < leftHandPaths.Count - 1)
                        {
                            currentLeftPathIndex++;
                            Debug.Log(currentLeftPathIndex + " L");
                            GenerateLeftGhostHands(currentRightPathIndex);
                            sceneState = SceneState.STATE_NEXT;
                        }
                        if (currentRightPathIndex < rightHandPaths.Count - 1)
                        {
                            currentRightPathIndex++;
                            Debug.Log(currentLeftPathIndex + " R");
                            GenerateRightGhostHands(currentRightPathIndex);
                            sceneState = SceneState.STATE_NEXT;
                        }
                        
                        break;
                }
            } 
        }
    }
    void OnPathTraversed()
    {
        Debug.Log("next");
        sceneState = SceneState.STATE_DONE;
    }
    void SetPath(GhostHandPathSO handPathSO, GameObject ghostHand)
    {
        List<GameObject> newPath = new List<GameObject>();
        foreach (int i in handPathSO.pathWaypoints)
        {
            newPath.Add(facepoints[i]);
        }
        ghostHand.GetComponent<GhostHandMassageMovementController>().SetMassagePath(newPath, handPathSO.maxPathTime);
    }
    void StartGenerateGhostHands()
    {
        GenerateLeftGhostHands(0);
        GenerateRightGhostHands(0);
    }
    void GenerateLeftGhostHands(int index)
    {
        leftHand = Instantiate(leftHandPaths[index].pathObject);
        SetPath(leftHandPaths[index], leftHand);
        leftHand.GetComponent<GhostHandMassageMovementController>().StartMoving();
    }
    void GenerateRightGhostHands(int index)
    {
        rightHand = Instantiate(rightHandPaths[index].pathObject);
        SetPath(rightHandPaths[index], rightHand);
        rightHand.GetComponent<GhostHandMassageMovementController>().StartMoving();
    }
}

