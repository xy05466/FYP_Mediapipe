using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class RhythmChartSO : ScriptableObject
{
    public TextAsset textAssetData;
    public AudioClip song;
    [System.Serializable]
    public class Note
    {
        public int index;
        public float presstime;
        public float x;
        public float y;
        public int nextIndex;

    }
    [System.Serializable]
    public class NoteChart
    {
        public Note[] notes;
    }

    public NoteChart noteChart = new NoteChart();
}

