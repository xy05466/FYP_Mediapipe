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
            //run code for note
            //presstime to spawn 
            //positino to spawn 
            //next index
            /*GameObject gameObject = *///Instantiate(notePrefab, Vector3.zero, Quaternion.identity).transform.SetParent(parent.transform);
            //gameObject.transform.SetParent(parent.transform);
            //gameObject.transform.set
            //gameObject.transform.localPosition = new Vector3(notes[nextIndex].x, notes[nextIndex].y, 0);
            //newObject.transform.localPosition = new Vector3(notes[nextIndex].x, notes[nextIndex].y, 0);
            //Instantiate(notePrefab, new Vector3(notes[nextIndex].x, notes[nextIndex].y, 0), Quaternion.identity, parent.transform);
            //(Instantiate(notePrefab, new Vector3(notes[nextIndex].x, notes[nextIndex].y,0), Quaternion.identity) as GameObject).transform.parent = parent.transform;
            Vector3 parentPosition = parent.transform.position; // Get the parent's position
            Vector3 spawnPosition = Vector3.zero; // Set the position relative to the parent

            // Adjust the spawn position relative to the parent's position
            spawnPosition += parentPosition;

            Instantiate(notePrefab, spawnPosition, Quaternion.identity).transform.SetParent(parent.transform);
        }
        else
        {
            
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
