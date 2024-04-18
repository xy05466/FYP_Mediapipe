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
    [SerializeField] List<GameObject> petals;

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
        creamApplicationGuideline.SetActive(true);

        creamApplicationDiagram.SetActive(false);
        //nextSceneButton.SetActive(false);
        headerTextBox.text = "Measure the right amount of cream using the guideline below.\nKeep your palm open.";
        faceDetected = false;
        flowerSet = false;
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
                            GameObject newPetal = Instantiate(petals[Random.Range(0, petals.Count)], temp[j].transform);
                            newPetal.transform.localScale = new Vector3(10, 10, 10);
                            newPetal.transform.localPosition = new Vector3(0, 0, -10);
                            petals.Add(newPetal);
                        }
                        flowerSet = true;
                    }
                    else
                    {
                        foreach (GameObject petal in petals)
                        {
                            Accupoint collDect = petal.GetComponent<Accupoint>();
                            if (collDect != null)
                            {
                                if (collDect.colliding)
                                {

                                }
                            }
                        }
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
