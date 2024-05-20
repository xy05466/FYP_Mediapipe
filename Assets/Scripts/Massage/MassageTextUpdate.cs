using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MassageTextUpdate : MonoBehaviour
{
    TMP_Text tMP_Text;
    [SerializeField]
    string text;
    private void Start()
    {
        tMP_Text = GetComponent<TMP_Text>();
        SilderSet.onUpdateMassageUI += UpdateText;
    }
    private void OnDestroy()
    {
        SilderSet.onUpdateMassageUI -= UpdateText;

    }
    public void UpdateText(string value)
    {
        tMP_Text.text = text + value;
    }    
}
