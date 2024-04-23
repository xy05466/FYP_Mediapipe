using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class CSVRhythmChartReader : MonoBehaviour
{
    public RhythmChartSO rhythmChartSO;
    //public RhythmChartSO rhythmChartSO;

    public void Start()
    {
        ReadRhythmChart();
    }
    public void ReadRhythmChart()
    {
        string[] lines = rhythmChartSO.textAssetData.text.Split(new string[] { "\n" }, StringSplitOptions.None);

        int curr = 0;
        rhythmChartSO.noteChart.notes = new RhythmChartSO.Note[lines.Length - 2];
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] data = lines[i].Split(",");
            var notes = rhythmChartSO.noteChart.notes;
            notes[curr] = new RhythmChartSO.Note();
            notes[curr].index = int.Parse(data[0]);
            notes[curr].presstime = float.Parse(data[1]);
            notes[curr].x = float.Parse(data[2]);
            notes[curr].y = float.Parse(data[3]);
            notes[curr].nextIndex = int.Parse(data[4]);
            curr++;
        }
    }

}
