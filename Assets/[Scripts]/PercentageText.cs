using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PercentageText : MonoBehaviour
{
    private TMP_Text textbox;
    string newText;
    private void Awake()
    {
        textbox = GetComponent<TMP_Text>();
        textbox.text = "Percentage: 0";
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
