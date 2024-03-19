using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CSVReader : MonoBehaviour
{

    public TextAsset textAssetData;
    public int numHeaders;
    [System.Serializable]
    public class Dialouge
    {
        public int id;
        public string desc;
        public string dialougeText;
        
    }
    [System.Serializable]
    public class DialougeList
    {
        public Dialouge[] dialouge;
    }

    public DialougeList dialougeList = new DialougeList();

    private void Awake()
    {
        //GameObject[] objs = GameObject.FindGameObjectsWithTag("csvReader");

        //if (objs.Length > 1)
        //{
        //    Destroy(this.gameObject);
        //}

        //DontDestroyOnLoad(this.gameObject);
        ReadCSV();
    }
    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
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

        int tableSize = data.Length / numHeaders - 1;
        dialougeList.dialouge = new Dialouge[tableSize];
    
        for(int i = 0; i < tableSize; i++)
        {
            dialougeList.dialouge[i] = new Dialouge();
            dialougeList.dialouge[i].id = int.Parse(data[numHeaders * (i + 1)]);
            dialougeList.dialouge[i].desc = data[numHeaders * (i + 1) + 1];
            dialougeList.dialouge[i].dialougeText = data[numHeaders * (i + 1) + 2];
        }
    }
}
