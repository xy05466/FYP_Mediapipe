using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class QuizSO : ScriptableObject
{
    [SerializeField]
    private int a1Score { get; set; }
    
    [SerializeField]
    private int a2Score { get; set; }

    [SerializeField]
    private int a3Score { get; set; }

    [SerializeField]
    private int a4Score { get; set; }

    private bool quizDone { get; set; }

    public void SetA1Score(int i)
    {
        a1Score += i;
    }

    public void SetA2Score(int i)
    {
        a2Score += i;
    }

    public void SetA3Score(int i)
    {
        a3Score += i;
    }
    public void SetA4Score(int i)
    {
        a4Score += i;
    }

    public void SetQuizDone(bool isDone)
    {
        quizDone = isDone;
    }

    public int GetA1Score()
    { 
        return a1Score; 
    }

    public int GetA2Score()
    {
        return a2Score;
    }

    public int GetA3Score()
    {
        return a3Score;
    }

    public int GetA4Score()
    {
        return a4Score;
    }

    public bool GetQuizDone()
    {
        return quizDone;
    }
}
