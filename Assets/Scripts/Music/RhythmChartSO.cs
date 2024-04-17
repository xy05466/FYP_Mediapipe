using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class RhythmChartSO : ScriptableObject
{
    public TextAsset textAssetData;
    [System.Serializable]
    public class Note
    {
        public int id;
        public string desc;
        public string dialougeText;

    }
    [System.Serializable]
    public class NoteChart
    {
        public Note[] noteList;
    }

    public NoteChart noteChart = new NoteChart();
}

