using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mediapipe;

public class FaceDistanceController : MonoBehaviour
{
  [SerializeField] private TMP_Text textbox;
  string newText;
  private void Awake()
  {
    //textbox = GetComponent<TMP_Text>();
    textbox.text = "No face detected.";
  }
  public void UpdateTextBox(string newText)
  {
    this.newText = newText;
  }
  private void Update()
  {
    textbox.text = newText;
  }
  public void PrintFaceGestures(List<Detection> value)
  {
    int counter = 0;
    foreach (Detection detection in value)
    {
      counter++;
      textbox.text = "Detection " + counter + ": " + detection.DisplayName;
    }
  }
}
