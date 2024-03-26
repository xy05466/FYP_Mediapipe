using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizSO quizSO;
    [SerializeField] private TMP_Text quizQn;
    [SerializeField] private TMP_Text quizA1;
    [SerializeField] private TMP_Text quizA2;
    [SerializeField] private TMP_Text quizA3;
    [SerializeField] private TMP_Text quizA4;
    [SerializeField] private TMP_Text quizResult;
    [SerializeField] private GameObject quizEnd;
    [SerializeField] private GameObject quizQnA;

    private int quizCount;

    public GameObject A1Button;
    public GameObject A2Button;
    public GameObject A3Button;
    public GameObject A4Button;
    public Button nextButton;
    private GameObject selectedButton;

    // Start is called before the first frame update
    void Start()
    {
        quizCount = 0;
        nextButton.interactable = false;
    }

    private void Update()
    {
       
    }

    private void ShowQuizResult()
    {
        List<int> quizWeights = new List<int>();
        quizWeights.Add(quizSO.GetA1Score());
        quizWeights.Add(quizSO.GetA2Score());
        quizWeights.Add(quizSO.GetA3Score());
        quizWeights.Add(quizSO.GetA4Score());

        switch (quizWeights.IndexOf(quizWeights.Max()))
        {
            case 0:
                quizResult.text = "Result 1";
                break;
            case 1:
                quizResult.text = "Result 2";
                break; 
            case 2:
                quizResult.text = "Result 3";
                break;
            case 3:
                quizResult.text = "Result 4";
                break;
        }
    }

    public void ChangeQn()
    {
        quizCount++;

        if (selectedButton == A1Button)
        {
            Debug.Log(A1Button.name + " was selected");
            quizSO.SetA1Score(1);
        }
        if (selectedButton == A2Button)
        {
            Debug.Log(A2Button.name + " was selected");
            quizSO.SetA2Score(1);
        }
        if (selectedButton == A3Button)
        {
            Debug.Log(A3Button.name + " was selected");
            quizSO.SetA3Score(1);
        }
        if (selectedButton == A4Button)
        {
            Debug.Log(A4Button.name + " was selected");
            quizSO.SetA4Score(1);
        }

        switch (quizCount)
        {
            case 1:
                quizQn.text = "Question 2";
                quizA1.text = "Answer 1 - 2";
                quizA2.text = "Answer 2 - 2";
                quizA3.text = "Answer 3 - 2";
                quizA4.text = "Answer 4 - 2";
                break;
            case 2:
                quizQn.text = "Question 3";
                quizA1.text = "Answer 1 - 3";
                quizA2.text = "Answer 2 - 3";
                quizA3.text = "Answer 3 - 3";
                quizA4.text = "Answer 4 - 3";
                break;
            case 3:
                quizQn.text = "Question 4";
                quizA1.text = "Answer 1 - 4";
                quizA2.text = "Answer 2 - 4";
                quizA3.text = "Answer 3 - 4";
                quizA4.text = "Answer 4 - 4";
                break;
            case 4:
                quizQn.text = "Question 5";
                quizA1.text = "Answer 1 - 5";
                quizA2.text = "Answer 2 - 5";
                quizA3.text = "Answer 3 - 5";
                quizA4.text = "Answer 4 - 5";
                break;
            case 5:
                quizSO.SetQuizDone(true);
                quizQnA.SetActive(false);
                quizEnd.SetActive(true);
                ShowQuizResult();
                quizResult.gameObject.SetActive(true);
                Debug.Log("quizdone");
                break;
        }
        Debug.Log(quizSO.GetA1Score());
        Debug.Log(quizSO.GetA2Score());
        Debug.Log(quizSO.GetA3Score());
        Debug.Log(quizSO.GetA4Score());
        nextButton.interactable = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnClick()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name + " was selected");
        selectedButton = EventSystem.current.currentSelectedGameObject;
        nextButton.interactable = true;
    }

    //public void OnClickA1()
    //{
    //    quizSO.SetA1Score(1);
    //}

    //public void OnClickA2()
    //{
    //    quizSO.SetA2Score(1);
    //}
    //public void OnClickA3()
    //{
    //    quizSO.SetA3Score(1);
    //}
    //public void OnClickA4()
    //{
    //    quizSO.SetA4Score(1);
    //}
}
