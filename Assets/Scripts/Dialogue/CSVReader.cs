using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CSVReader : MonoBehaviour
{

    public DialougeListSO dialougeListSO;

    public void ReadCSV()
    {
        string[] data = dialougeListSO.textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        for(int i = 0; i < data.Length; i++)
        {
            if (data[i].EndsWith("|"))
            {
                if (i < data.Length - 1)
                {
                    //adds the comma back in
                    data[i] += "," + data[i + 1];
                    // Remove the next part from the array
                    data = data.Where((val, idx) => idx != i + 1).ToArray();
                    //removes "|" if it exist in the string
                    data[i] = data[i].Replace("|", "");
                }
            }
            //removes all double quatations
            data[i] = data[i].Replace("\"", "");
        }

        int tableSize = data.Length / dialougeListSO.numHeaders - 1;
        dialougeListSO.dialougeList.dialouge = new DialougeListSO.Dialouge[tableSize];
    
        for(int i = 0; i < tableSize; i++)
        {
            dialougeListSO.dialougeList.dialouge[i] = new DialougeListSO.Dialouge();
            dialougeListSO.dialougeList.dialouge[i].id = int.Parse(data[dialougeListSO.numHeaders * (i + 1)]);
            dialougeListSO.dialougeList.dialouge[i].desc = data[dialougeListSO.numHeaders * (i + 1) + 1];
            dialougeListSO.dialougeList.dialouge[i].dialougeText = data[dialougeListSO.numHeaders * (i + 1) + 2];
        }
    }
}
