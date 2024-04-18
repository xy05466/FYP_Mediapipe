using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreamSceneManager : MonoBehaviour
{
    [SerializeField] HandPointManager hpm;
    [SerializeField] GameObject handDetectionSolution;
    [SerializeField] GameObject handDetectionScreen;
    [SerializeField] GameObject creamApplicationGuideline;

    [SerializeField] GameObject creamApplicationDiagram;
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
    List<GameObject> smallPetals;
     
    // all points with small petal
    List<int> foreheadPoints = new List<int>   {  63,  68,  34, 103, 104, 105,  67,  69,  66, 109, 108, 107,  10, 151,   9, 338, 337, 336, 297, 299, 296, 332, 333, 334, 284, 298, 293 };
    List<int> rightCheekPoints = new List<int> { 111, 117, 118, 119, 120, 121,  47, 100, 101,  50, 123, 187, 205,  36, 142 };
    List<int> leftCheekPoints = new List<int>  { 340, 346, 347, 348, 349, 350, 277, 329, 330, 280, 352, 411, 425, 266, 371 };
    List<int> chinCheekPoints = new List<int>  { 212,  57, 202,  43, 204, 106, 194, 182, 201,  83,  18, 200, 313, 421, 406, 335, 424, 273, 422, 287, 432 };

    GameObject palmBase;
    float palmDistance;
    bool faceDetected;
    bool flowerSet;
    enum SceneState
    {
        STATE_MEASURE,
        STATE_APPLY
    }
    SceneState sceneState;
    private void Awake()
    {
        sceneState = SceneState.STATE_MEASURE;
        handDetectionSolution.SetActive(true);
        handDetectionScreen.SetActive(true);
        //creamApplicationGuideline.SetActive(true);
        //creamApplicationDiagram.SetActive(false);
        //nextSceneButton.SetActive(false);
        headerTextBox.text = "Measure the right amount of cream using the guideline below.\nKeep your palm open.";
        faceDetected = false;
        flowerSet = false;
        bigPetals = new List<GameObject>();
        smallPetals = new List<GameObject>();
    }
    private void Update()
    {
        if (sceneState == SceneState.STATE_MEASURE)
        {
            
            if (hpm != null)
            {
                
                GameObject[] temp = hpm.GetFacePointTransforms();
                if (temp != null && temp.Length > 0)
                {
                    palmBase = temp[4];
                }
            }
            if (palmBase != null)
            {
                palmDistance = (palmBase.transform.position.z - Camera.main.transform.position.z);
                headerTextBox.text = palmDistance.ToString();
                if (palmDistance > maxDistance)
                {
                    headerTextBox.text += "\nPlease move your hand closer.";
                }
                else if (palmDistance < minDistance)
                {
                    headerTextBox.text += "\nPlease move your hand further.";
                }
            }
            if (fpm != null)
            {
                GameObject[] temp = fpm.GetFacePointTransforms();
                if (temp != null && temp.Length > 0)
                {
                    if (!flowerSet)
                    {
                        foreach (int j in petalFaceLandmarks)
                        {
                            GameObject newPetal = Instantiate(bigPetalPrefabs[Random.Range(0, bigPetalPrefabs.Count)], temp[j].transform);
                            newPetal.transform.localScale = new Vector3(10, 10, 10);
                            newPetal.transform.localPosition = new Vector3(0, 0, -10);
                            bigPetals.Add(newPetal);
                        }
                        foreach (int j in foreheadPoints)
                        {
                            GameObject newPetal = Instantiate(smallPetalPrefabs[Random.Range(0, smallPetalPrefabs.Count)], temp[j].transform);
                            newPetal.transform.localScale = new Vector3(3, 3, 3);
                            newPetal.transform.localPosition = new Vector3(0, 0, -10);
                            smallPetals.Add(newPetal);
                        }
                        foreach (int j in leftCheekPoints)
                        {
                            GameObject newPetal = Instantiate(smallPetalPrefabs[Random.Range(0, smallPetalPrefabs.Count)], temp[j].transform);
                            newPetal.transform.localScale = new Vector3(3, 3, 3);
                            newPetal.transform.localPosition = new Vector3(0, 0, -10);
                            smallPetals.Add(newPetal);
                        }
                        foreach (int j in rightCheekPoints)
                        {
                            GameObject newPetal = Instantiate(smallPetalPrefabs[Random.Range(0, smallPetalPrefabs.Count)], temp[j].transform);
                            newPetal.transform.localScale = new Vector3(3, 3, 3);
                            newPetal.transform.localPosition = new Vector3(0, 0, -10);
                            smallPetals.Add(newPetal);
                        }
                        foreach (int j in chinCheekPoints)
                        {
                            GameObject newPetal = Instantiate(smallPetalPrefabs[Random.Range(0, smallPetalPrefabs.Count)], temp[j].transform);
                            newPetal.transform.localScale = new Vector3(3, 3, 3);
                            newPetal.transform.localPosition = new Vector3(0, 0, -10);
                            smallPetals.Add(newPetal);
                        }
                        flowerSet = true;
                    }
                }
            }
        }
    }
    public void CreamApplied()
    {
        handDetectionSolution.SetActive(false);
        handDetectionScreen.SetActive(false);
        creamApplicationGuideline.SetActive(false);

        creamApplicationDiagram.SetActive(true);
        nextSceneButton.SetActive(true);
    }
}
