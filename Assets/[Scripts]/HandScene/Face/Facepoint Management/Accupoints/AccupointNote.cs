using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccupointNote : MonoBehaviour//this whole thing can ask from paul 
{
    //ring closing 
    //neeed to compare spawn time and ideal time
    public int index;
    public float presstime;
    public AudioSource audioSource;
    //time pressed
    public delegate void OnRhythmPress();
    // Declare a delegate variable
    public static event OnRhythmPress onRhythmPress;
    private void Update()
    {
        if (audioSource.time > presstime + 1)
        {
            RhythmGameController.currIndex = index;
            onRhythmPress?.Invoke();
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        RhythmGameController.currIndex = index;
        onRhythmPress?.Invoke();
        Destroy(gameObject);
    }
}
