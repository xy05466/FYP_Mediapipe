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
    [SerializeField] List<GhostHandPathSO> leftUIPaths;
    [SerializeField] List<GhostHandPathSO> rightUIPaths;

    [SerializeField] GameObject handParent;

    [SerializeField] GameObject startButton;
    [SerializeField] GameObject nextButton;


    public static UnityEvent FinishedPathEvent;

    GameObject leftHand;
    GameObject rightHand;
    GameObject leftPath;
    GameObject rightPath;

    GameObject[] handpoints;
    GameObject[] facepoints;

    int currentLeftPathIndex;
    int currentRightPathIndex;

    bool canSpawnHands;
    enum SceneState
    {
        STATE_SPLASH,
        STATE_START,
        STATE_WAIT,
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
        startButton.SetActive(false);
        nextButton.SetActive(false);
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
                        startButton.SetActive(true);
                        nextButton.SetActive(false);
                        break;
                    case SceneState.STATE_WAIT:
                        break;
                    case SceneState.STATE_NEXT:
                        if (leftPath != null)
                            SetUIPath(leftHandPaths[currentLeftPathIndex], leftPath);
                        if (rightPath != null)
                            SetUIPath(rightHandPaths[currentRightPathIndex], rightPath);
                        break;
                    case SceneState.STATE_DONE:
                        nextButton.SetActive(true);
                        startButton.SetActive(false);
                        if (leftHand != null)
                            Destroy(leftHand);
                        if (rightHand != null)
                            Destroy(rightHand);
                        if (leftPath != null)
                            Destroy(leftPath);
                        if (rightPath != null)
                            Destroy(rightPath);
                        break;
                }
            }
        }
    }
    public void LeaveSplash()
    {
        sceneState = SceneState.STATE_START;
    }
    public void StartGeneratePath()
    {
        StartGenerateGhostHands();
        sceneState = SceneState.STATE_NEXT;
    }
    public void StartNextPath()
    {
        
        if (currentLeftPathIndex < leftHandPaths.Count - 1)
        {
            currentLeftPathIndex++;
            Debug.Log(currentLeftPathIndex + " L");
            GenerateLeftGhostHands(currentLeftPathIndex);
            if (currentLeftPathIndex < leftUIPaths.Count - 1)
            {
                GenerateLeftUIPaths(currentLeftPathIndex);
            }
            sceneState = SceneState.STATE_NEXT;

        }
        if (currentRightPathIndex < rightHandPaths.Count - 1)
        {
            currentRightPathIndex++;
            Debug.Log(currentLeftPathIndex + " R");
            GenerateRightGhostHands(currentRightPathIndex);
            if (currentRightPathIndex < rightHandPaths.Count - 1)
            {
                GenerateRightUIPaths(currentRightPathIndex);
            }
            sceneState = SceneState.STATE_NEXT;
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
        ghostHand?.GetComponent<GhostHandMassageMovementController>()?.SetMassagePath(newPath, handPathSO.maxPathTime);
    }
    void SetUIPath(GhostHandPathSO handPathSO, GameObject uiPath)
    {
        List<Vector3> newPath = new List<Vector3>();
        foreach (int i in handPathSO.pathWaypoints)
        {
            newPath.Add(new Vector3(facepoints[i].transform.position.x, facepoints[i].transform.position.y, 0));
        }
        uiPath.GetComponent<LineRenderer>().positionCount = newPath.Count;
        uiPath.GetComponent<LineRenderer>().SetPositions(newPath.ToArray());
    }
    void StartGenerateGhostHands()
    {
        GenerateLeftGhostHands(0);
        GenerateRightGhostHands(0);
        GenerateLeftUIPaths(0);
        GenerateRightUIPaths(0);
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
    void GenerateLeftUIPaths(int index)
    {
        leftPath = Instantiate(leftUIPaths[index].pathObject, new Vector3(0, 0, 10f), Quaternion.identity);
        leftPath.transform.SetParent(handParent.transform);
        SetUIPath(leftHandPaths[index], leftPath);
        leftPath.GetComponent<LineRenderer>().alignment = LineAlignment.View;
    }
    void GenerateRightUIPaths(int index)
    {
        rightPath = Instantiate(rightUIPaths[index].pathObject, new Vector3(0, 0, 10f), Quaternion.identity);
        rightPath.transform.SetParent(handParent.transform);
        SetUIPath(rightHandPaths[index], rightPath);
        rightPath.GetComponent<LineRenderer>().alignment = LineAlignment.View;
    }
}

