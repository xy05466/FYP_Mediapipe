using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreamSceneManager : MonoBehaviour
{
    public float fadeSpeed = 1f;

    //[SerializeField] TMP_Text headerTextBox;
    //[SerializeField] private float idealDistance = 2.0f;
    //[SerializeField] float minDistance = 115f;
    //[SerializeField] float maxDistance = 90f;
    [SerializeField] List<int> petalFaceLandmarks;
    [SerializeField] List<GameObject> bigPetalPrefabs;
    [SerializeField] List<GameObject> smallPetalPrefabs;

    List<FlowerAccupoint> bigPetals = new List<FlowerAccupoint>();
    public List<FlowerAccupoint> allPetals = new List<FlowerAccupoint>();
    List<List<FlowerAccupoint>> smallPetals = new List<List<FlowerAccupoint>>();
    [SerializeField] GameObject mainFlowerParent;
     
    // all points with small petal
    List<int> foreheadPoints = new List<int>   { 63, 68, 34, 103, 104, 105, 67, 69, 66, 109, 108, 107, 10, 151, 9, 338, 337, 336, 297, 299, 296, 332, 333, 334, 284, 298, 293 };
    List<int> rightCheekPoints = new List<int> { 111, 117, 118, 119, 120, 121, 47, 100, 101, 50, 123, 187, 205, 36, 142 };
    List<int> leftCheekPoints = new List<int>  { 340, 346, 347, 348, 349, 350, 277, 329, 330, 280, 352, 411, 425, 266, 371 };
    List<int> chinCheekPoints = new List<int>  { 212, 57, 202, 43, 204, 106, 194, 182, 201, 83, 18, 200, 313, 421, 406, 335, 424, 273, 422, 287, 432 };
    List<List<int>> tempIntList;

    enum SceneState
    {
        STATE_MEASURE,
        STATE_APPLY
    }
    SceneState sceneState;
    public void Init()
    {
        tempIntList = new List<List<int>>() { foreheadPoints, leftCheekPoints, rightCheekPoints, chinCheekPoints };
        sceneState = SceneState.STATE_MEASURE;
        //handDetectionSolution.SetActive(true);
        //handDetectionScreen.SetActive(true);
        for (int i = 0; i < 4; ++i)
        {
            smallPetals.Add(new List<FlowerAccupoint>());
        }
        // spawn big petals
        foreach (int j in petalFaceLandmarks)
        {
            FlowerAccupoint newPetal = Instantiate(bigPetalPrefabs[Random.Range(0, bigPetalPrefabs.Count)], mainFlowerParent.transform).GetComponent<FlowerAccupoint>();
            newPetal.fadeSpeed = fadeSpeed;
            newPetal.pointId = j;
            newPetal.transform.localScale *= 25;
            newPetal.transform.localPosition = new Vector3(0, 0, -10);
            bigPetals.Add(newPetal);
            allPetals.Add(newPetal);
        }
        // small petal init
        for (int i = 0; i < tempIntList.Count; ++i)
        {
            for (int j = 0; j  < tempIntList[i].Count; j++)
            {
                FlowerAccupoint newPetal = Instantiate(smallPetalPrefabs[Random.Range(0, bigPetalPrefabs.Count)], mainFlowerParent.transform).GetComponent<FlowerAccupoint>();
                newPetal.pointId = tempIntList[i][j];
                newPetal.transform.localScale *= 10;
                newPetal.transform.localPosition = new Vector3(0, 0, -10);
                smallPetals[i].Add(newPetal);
                allPetals.Add(newPetal);
            }
        }
    }
    
}
