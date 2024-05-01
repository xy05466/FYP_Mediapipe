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
    [SerializeField]
    private GameObject sliderPrefab;
    //allowance (min value will be 
    [Header("Good Bad Fail")]
    [SerializeField]
    private Vector3 allowance;
    //spawn offset
    [SerializeField]
    private float spawnOffset = 0.3f;

    List<float> spawnTimeList;
    //List<float> SlidersToSpawn;
    //int SpawnNextIndex = 0;
    //bool SlidersInQueue = false;

    //Event for OnExitSlider
    //public delegate void OnStartMoveSlider();
    //public static event OnStartMoveSlider onStartMoveSlider;

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
        //SlidersToSpawn = new List<float>();
        musicSystem.soundClip = rhythmChartSO.song;
        AccupointNote.onRhythmPress += OnNotePress;
        //AccupointSlider.onRhythmSliderEnter += OnSliderEnter;
        //AccupointSlider.onRhythmSliderExit += OnSliderExit;
        for (var i = 0; i < rhythmChartSO.noteChart.notes.Length; i++)
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
        //if (SlidersInQueue)
        //{
        //    if (SpawnNextIndex > SlidersToSpawn.Count - 1)
        //    {
        //        SlidersInQueue = false;
        //        return;
        //    }
        //    //if(SpawnNextIndex > SlidersToSpawn.Count)
        //    if (musicSystem.audioSource.time > SlidersToSpawn[SpawnNextIndex])
        //    {
        //        onStartMoveSlider?.Invoke();
        //        SpawnNextIndex++;
                
        //    }
        //}
    }
    //when to spawn 
    //how to read the data
    public void processNotes()
    {
        var notes = rhythmChartSO.noteChart.notes;
        int localNextIndex = notes[nextIndex].nextIndex;
        if (localNextIndex == -1)
        {
            GameObject newNote = Instantiate(notePrefab, parent.transform);
            newNote.transform.localPosition = new Vector3(notes[nextIndex].x, notes[nextIndex].y, 0);
            AccupointNote accNote = newNote.GetComponent<AccupointNote>();
            accNote.index = nextIndex;
           

        }
        else//this means it is a slider 
        {
            GameObject newRhythmSlider = Instantiate(sliderPrefab, parent.transform);
            RhythmSlider rhythmSlider = newRhythmSlider.GetComponent<RhythmSlider>();
            //make a list for the way points
            RhythmChartSO.NoteChart points2Store = new RhythmChartSO.NoteChart();
            points2Store.notes = new RhythmChartSO.Note[4];
            addData2Store(points2Store, 0);
            for (var i = 0; i < 4; i++)
            {
                rhythmSlider.P[i].localPosition = new Vector3(points2Store.notes[i].x, points2Store.notes[i].y, 0);
            }
            //init speed here
            //calculate the start and end point of the slider and calculate the speed
            rhythmSlider.Init();
            float travelTime = points2Store.notes[3].presstime - (points2Store.notes[0].presstime - spawnOffset);

            rhythmSlider.speed = rhythmSlider.LUT[rhythmSlider.LUT.Length - 1] / travelTime;
            rhythmSlider.audioSource = musicSystem.audioSource;
            //add some timer here to make it so it can move 
            rhythmSlider.presstime = points2Store.notes[0].presstime;
            //SlidersInQueue = true;
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
    //for point scoring 
    public void OnSliderEnter()
    {

    }
    public void OnSliderExit()
    {

    }
}
