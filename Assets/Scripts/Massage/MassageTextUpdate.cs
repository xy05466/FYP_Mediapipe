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
        AccupointsMassageManagerFinal.onUpdateMassageUI += UpdateText;
    }
    private void OnDestroy()
    {
        AccupointsMassageManagerFinal.onUpdateMassageUI -= UpdateText;

    }
    public void UpdateText(string value)
    {
        tMP_Text.text = text + value;
    }
}
