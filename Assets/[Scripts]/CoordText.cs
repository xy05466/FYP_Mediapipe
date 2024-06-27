using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoordText : MonoBehaviour
{
    private TMP_Text textbox;
    string newText;
    private void Awake()
    {
        textbox = GetComponent<TMP_Text>();
        textbox.text = "Thumb: 0\nIndex: 0\nMiddle: 0\nRing: 0\nPinky:0";
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
