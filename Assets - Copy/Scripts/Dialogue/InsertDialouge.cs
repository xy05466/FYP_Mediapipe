using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InsertDialouge : MonoBehaviour
{
    [SerializeField]
    private TMP_Text DialougeBox;
    [SerializeField]
    private DialougeListSO dialougeListSO;
    [SerializeField]
    private int index = 0;

    private void Update()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        if (index > dialougeListSO.dialougeList.dialouge.Length)
            return;
        DialougeBox.text = dialougeListSO.dialougeList.dialouge[index].dialougeText;
    }
}
