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
        headerTextBox.text = "Measure the right amount of cream using the guideline below.";
    }
    private void Update()
    {
        if (sceneState == SceneState.STATE_MEASURE)
        {
            if (hpm != null)
            {
                GameObject[] temp = hpm.GetFacePointTransforms();
                if (temp != null)
                    palmBase = temp[0];
            }
            if (palmBase != null)
            {
<<<<<<< Updated upstream
                palmDistance = (palmBase.transform.position.z - Camera.main.transform.position.z) - 90;
=======
                palmDistance = (palmBase.transform.position.z - Camera.main.transform.position.z);
>>>>>>> Stashed changes
                headerTextBox.text = palmDistance.ToString();
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
