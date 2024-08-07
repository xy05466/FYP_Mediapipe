using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GestureText : MonoBehaviour
{
  private TMP_Text textbox;
  string newText;
  private void Awake()
  {
    textbox = GetComponent<TMP_Text>();
    textbox.text = "No gesture detected.";
  }
  public void UpdateTextBox(string newText)
  {
    this.newText = newText;
  }
  private void Update()
  {
    textbox.text = newText;
  }
}
