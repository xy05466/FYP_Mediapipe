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
    [SerializeField] private GameObject quizButtons;
    [SerializeField] private GameObject quizQnA;

    public GameObject A1Button;
    public GameObject A2Button;
    public GameObject A3Button;
    public GameObject A4Button;
    public Button nextButton;

    private GameObject selectedButton;
    private int quizCount;
    private int a1Score;
    private int a2Score;
    private int a3Score;
    private int a4Score;

    // Start is called before the first frame update
    void Start()
    {
        quizCount = 0;
        a1Score = 0;
        a2Score = 0;
        a3Score = 0;
        a4Score = 0;
    }

    private void Update()
    {
       if (EventSystem.current.currentSelectedGameObject == null)
       {
            nextButton.interactable = false;
       }
    }

    private void ShowQuizResult()
    {
        quizSO.SetA1Score(a1Score);
        quizSO.SetA2Score(a2Score);
        quizSO.SetA3Score(a3Score);
        quizSO.SetA4Score(a4Score);
        quizSO.SetQuizDone(true);
        quizQnA.SetActive(false);
        quizButtons.SetActive(true);

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
            a1Score++;
            //quizSO.SetA1Score(1);
        }
        if (selectedButton == A2Button)
        {
            Debug.Log(A2Button.name + " was selected");
            a2Score++;
            //quizSO.SetA2Score(1);
        }
        if (selectedButton == A3Button)
        {
            Debug.Log(A3Button.name + " was selected");
            a3Score++;
            //quizSO.SetA3Score(1);
        }
        if (selectedButton == A4Button)
        {
            Debug.Log(A4Button.name + " was selected");
            a4Score++;
            //quizSO.SetA4Score(1);
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
                ShowQuizResult();
                quizResult.gameObject.SetActive(true);
                Debug.Log("quizdone");
                break;
        }
        nextButton.interactable = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnClick()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name + " was selected");
        selectedButton = EventSystem.current.currentSelectedGameObject;
        nextButton.interactable = true;
    }
}
