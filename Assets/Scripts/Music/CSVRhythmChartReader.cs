using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class CSVRhythmChartReader : MonoBehaviour
{
    public RhythmChartSO rhythmChartSO;
    
    public void ReadRhythmChart()
    {
        string[] data = rhythmChartSO.textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        
        for(int i = 0; i < data.Length; i++)
        {
            int curr = 0;
            if (data[i] == "next")
            {
                //go next 
                curr++;
                return;
            }
            rhythmChartSO.noteChart.noteList[curr] = new RhythmChartSO.Note();
            rhythmChartSO.noteChart.noteList[curr].id = int.Parse(data[3 * (i + 1)]);
            rhythmChartSO.noteChart.noteList[curr].desc = data[3 * (i + 1) + 1];
            rhythmChartSO.noteChart.noteList[curr].dialougeText = data[3 * (i + 1) + 2];
        }
    }
}
