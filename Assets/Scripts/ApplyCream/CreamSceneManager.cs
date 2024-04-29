using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreamSceneManager : MonoBehaviour
{
    [SerializeField] HandPointManager hpm;
    [SerializeField] GameObject handDetectionSolution;
    [SerializeField] GameObject handDetectionScreen;
    //[SerializeField] GameObject creamApplicationGuideline;

    //[SerializeField] GameObject creamApplicationDiagram;
    [SerializeField] GameObject nextSceneButton;

    [SerializeField] TMP_Text headerTextBox;
    [SerializeField] private float idealDistance = 2.0f;

    [SerializeField] float minDistance = 115f;
    [SerializeField] float maxDistance = 90f;

    [SerializeField] FacepointManager fpm;
    [SerializeField] List<int> petalFaceLandmarks;
    [SerializeField] List<GameObject> bigPetalPrefabs;
    [SerializeField] List<GameObject> smallPetalPrefabs;

    List<GameObject> bigPetals;
    List<List<GameObject>> smallPetals;
    [SerializeField] GameObject mainFlowerParent;
    [SerializeField] float minThreshold = 88.3f;
     
    // all points with small petal
    List<int> foreheadPoints = new List<int>   { 63, 68, 34, 103, 104, 105, 67, 69, 66, 109, 108, 107, 10, 151, 9, 338, 337, 336, 297, 299, 296, 332, 333, 334, 284, 298, 293 };
    List<int> rightCheekPoints = new List<int> { 111, 117, 118, 119, 120, 121, 47, 100, 101, 50, 123, 187, 205, 36, 142 };
    List<int> leftCheekPoints = new List<int>  { 340, 346, 347, 348, 349, 350, 277, 329, 330, 280, 352, 411, 425, 266, 371 };
    List<int> chinCheekPoints = new List<int>  { 212, 57, 202, 43, 204, 106, 194, 182, 201, 83, 18, 200, 313, 421, 406, 335, 424, 273, 422, 287, 432 };
    List<List<int>> tempIntList;

    GameObject nosePointRef;
    bool noseSet;
    float noseDistance;
    enum SceneState
    {
        STATE_MEASURE,
        STATE_APPLY
    }
    SceneState sceneState;
    private void Awake()
    {
        tempIntList = new List<List<int>>() { foreheadPoints, leftCheekPoints, rightCheekPoints, chinCheekPoints };
        sceneState = SceneState.STATE_MEASURE;
        handDetectionSolution.SetActive(true);
        handDetectionScreen.SetActive(true);
        noseSet = false;
        bigPetals = new List<GameObject>();
        smallPetals = new List<List<GameObject>>();
        for (int i = 0; i < 4; ++i)
        {
            smallPetals.Add(new List<GameObject>());
        }
        foreach (int j in petalFaceLandmarks)
        {
            GameObject newPetal = Instantiate(bigPetalPrefabs[Random.Range(0, bigPetalPrefabs.Count)], mainFlowerParent.transform);
            newPetal.transform.localScale = new Vector3(25, 25, 25);
            newPetal.transform.localPosition = new Vector3(0, 0, -10);
            bigPetals.Add(newPetal);
        }
        // small petal init
        
        for (int i = 0; i < tempIntList.Count; ++i)
        {
            for (int j = 0; j  < tempIntList[i].Count; j++)
            {
                GameObject newPetal = Instantiate(smallPetalPrefabs[Random.Range(0, bigPetalPrefabs.Count)], mainFlowerParent.transform);
                newPetal.transform.localScale = new Vector3(10, 10, 10);
                newPetal.transform.localPosition = new Vector3(0, 0, -10);
                smallPetals[i].Add(newPetal);
            }
        }
    }
    private void Update()
    {
        int counter = 0;
        if (hpm == null)
        {
            headerTextBox.text = "Let's apply our facial cream.\nApply the cream by putting it on your hands and spreading it from the petal.\nNo hand detected!";
        }
        else
        {
            headerTextBox.text = "Let's apply our facial cream.\nApply the cream by putting it on your hands and spreading it from the petal.";
        }
        // 1. Get Face Points
        if (fpm == null) return;
        if (fpm.isActiveAndEnabled)
        {
            GameObject[] facepoints = fpm.GetFacePointTransforms();
           
            if (facepoints != null)
            {
                if (facepoints.Length > 0)
                {
                    if (!noseSet)
                    {
                        nosePointRef = facepoints[4];
                        noseSet = true;
                    }
                    else
                    {
                        noseDistance = nosePointRef.transform.position.z;
                        headerTextBox.text = "Face distance: " + noseDistance;
                    }
                    // 2. Place flowers at correct position.
                    for (int h = 0; h < bigPetals.Count; ++h)
                    {
                        bigPetals[h].transform.position = facepoints[petalFaceLandmarks[h]].transform.position;
                        bigPetals[h].transform.localPosition = new Vector3(bigPetals[h].transform.localPosition.x, bigPetals[h].transform.localPosition.y, -10);
                    }
                    for (int i = 0; i < smallPetals.Count; ++i)
                    {
                        for (int j = 0; j < smallPetals[i].Count; ++j)
                        {
                            Vector3 nextFlowerPosition = facepoints[tempIntList[i][j]].transform.position;
                            smallPetals[i][j].transform.position = nextFlowerPosition;
                            smallPetals[i][j].transform.localPosition = new Vector3(smallPetals[i][j].transform.localPosition.x, smallPetals[i][j].transform.localPosition.y, -1);
                            FlowerAccupoint flowerAccupoint = smallPetals[i][j].GetComponentInChildren<FlowerAccupoint>();
                            if (noseDistance > minThreshold)
                            {
                                smallPetals[i][j].transform.localScale = new Vector3(15, 15, 15);
                            }
                            else
                            {
                                smallPetals[i][j].transform.localScale = new Vector3(10, 10, 10);
                            }
                            if (flowerAccupoint != null)
                            {
                                if (flowerAccupoint.collided)
                                {
                                    counter++;
                                    //Debug.Log(counter);
                                }
                            }
                        }
                    }
                }
            }
        }
        if (Mathf.Abs(counter - (foreheadPoints.Count + rightCheekPoints.Count + leftCheekPoints.Count + chinCheekPoints.Count)) < 25)
        {
            nextSceneButton.SetActive(true);
        }
    }
    public void CreamApplied()
    {
        handDetectionSolution.SetActive(false);
        handDetectionScreen.SetActive(false);
        //creamApplicationGuideline.SetActive(false);

        //creamApplicationDiagram.SetActive(true);
        nextSceneButton.SetActive(true);
    }
}
