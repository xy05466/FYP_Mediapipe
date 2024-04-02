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

    GameObject palmBase;
    float palmDistance;
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
        nextSceneButton.SetActive(false);
        headerTextBox.text = "Measure the right amount of cream using the guideline below.\nKeep your palm open.";
    }
    private void Update()
    {
        if (sceneState == SceneState.STATE_MEASURE)
        {
            Debug.Log(1);
            if (hpm != null)
            {
                Debug.Log(2);
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
                Debug.Log(4);
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
