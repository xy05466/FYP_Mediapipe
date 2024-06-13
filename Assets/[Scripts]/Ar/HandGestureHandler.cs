using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGestureHandler : MonoBehaviour
{
    public string currGesture;
    //handle inputs here
    //maybe can use event system
    void HandleGestures()
    {
        switch (currGesture)
        {
            case "Peace":
                Debug.Log("Choice 1 selected.");
                break;
            case "Pakyu":
                Debug.Log("Choice 2 selected.");
                break;
            case "Next":
                Debug.Log("Choice 3 selected.");
                break;
            default:
                Debug.Log("Invalid choice.");
                break;
        }
    }
}
