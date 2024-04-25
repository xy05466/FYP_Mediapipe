using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RhythmGameController : MonoBehaviour
{
    //connected systems
    [SerializeField]
    private AccupointAnchorManager Anchor;
    [SerializeField]
    private RhythmChartSO rhythmChartSO;
    [SerializeField]
    private MusicSystem musicSystem;
    //keep track of currindex
    static public int currIndex = 1;
    int nextIndex;
    //prefabs
    [SerializeField]
    private GameObject notePrefab;
    //allowance (min value will be 
    [Header("Good Bad Fail")]
    [SerializeField]
    private Vector3 allowance;
    //spawn offset
    [SerializeField]
    private float spawnOffset = 0.3f;

    List<float> spawnTimeList;
    

    public GameObject parent
    {
        get
        {
            if (Anchor.accupoint == null)
                return null;
            return Anchor.accupoint;
        }
    }
    private void Start()
    {
        spawnTimeList = new List<float>();
        musicSystem.soundClip = rhythmChartSO.song;
        AccupointNote.onRhythmPress += OnNotePress;
        for(var i = 0; i < rhythmChartSO.noteChart.notes.Length; i++)
        {

            float spawnTime = rhythmChartSO.noteChart.notes[i].presstime - spawnOffset;

            spawnTimeList.Add(spawnTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (parent == null)
            return;
        if (nextIndex > rhythmChartSO.noteChart.notes.Length - 1)
            return;
        if (musicSystem.audioSource.time > spawnTimeList[nextIndex])
        {
            processNotes();
            nextIndex++;
        }
    }
    //when to spawn 
    //how to read the data
    public void processNotes()
    {
        var notes = rhythmChartSO.noteChart.notes;
        int localNextIndex = notes[nextIndex].nextIndex;
        if(localNextIndex == -1) 
        {
            GameObject newNote = Instantiate(notePrefab, parent.transform);
            newNote.transform.localPosition = new Vector3(notes[nextIndex].x, notes[nextIndex].y, 0);
            AccupointNote accNote = newNote.GetComponent<AccupointNote>();
            accNote.index = nextIndex;
        }
        else//this means it is a slider 
        {
            //make a list for the way points
            RhythmChartSO.NoteChart points2Store =new RhythmChartSO.NoteChart();
            points2Store.notes = new RhythmChartSO.Note[5];
            addData2Store(points2Store, 0);
            for(var i = 0;i < points2Store.notes.Length - 1; i++)
                Debug.Log(points2Store.notes[i].index);
            //data is now stored in this list and you can edit it
            
        }
    }   
    private void addData2Store(RhythmChartSO.NoteChart points2Store, int currStore)
    {
        var notes = rhythmChartSO.noteChart.notes;
        //init the points to store
        points2Store.notes[currStore] = new RhythmChartSO.Note();
        //set data
        points2Store.notes[currStore] = notes[nextIndex];
        if (points2Store.notes[currStore].nextIndex == -1)
            return;
        else
        {
            currStore++;
            nextIndex++;
            addData2Store(points2Store, currStore);
        }
    }
    public void OnNotePress()
    {
        var notes = rhythmChartSO.noteChart.notes;
        float timePressed = musicSystem.audioSource.time;
        float idealTime = notes[currIndex].presstime;

        //calculate difference
        float difference = Mathf.Abs(idealTime - timePressed);
        if (difference < allowance.x)
        {
            //this is perfect
            Debug.Log("Good");
        }            
        else if (difference < allowance.y)
        {
            //this is bad
            Debug.Log("Bad");

        }
        else
        {
            //this is a fail 
            Debug.Log("Fail");

        }
    }

}
