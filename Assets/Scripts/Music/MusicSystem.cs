using UnityEngine;
using TMPro;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class MusicSystem : MonoBehaviour
{

    //sound controller
    [HideInInspector]
    public AudioClip soundClip; // Drag your audio clip here in the Unity editor
    [HideInInspector]
    public AudioSource audioSource;
    //Time
    private float clipLength;
    [SerializeField]
    private bool DebugMode;
    [Header("Debug Settings")]
    //UI Text
    public TMP_Text text_Time;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        audioSource.clip = soundClip; // Assign the audio clip to the AudioSource
        clipLength = soundClip.length;
        if (!DebugMode)
            return;
        text_Time.text = '0' + " / " + clipLength;
        slider.maxValue = clipLength;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!audioSource.isPlaying) // Check if the audio is not already playing
            {
                audioSource.Play(); // Play the audio clip
            }
        }
        if (audioSource.isPlaying)
        {
            Debug.Log(audioSource.time + " / " + clipLength);
            if (!DebugMode)
                return;
            text_Time.text = ConvertSecondsToMinutes((int)audioSource.time) + " / " + ConvertSecondsToMinutes((int)clipLength);

        }
    }
    string ConvertSecondsToMinutes(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        return minutes + ":" + remainingSeconds;
    }
    public void OnMoveSlider()
    {
        audioSource.time = slider.value;
        if (!audioSource.isPlaying) // Check if the audio is not already playing
        {
            audioSource.Play(); // Play the audio clip
        }
    }
    public void CheckIfOnTime(float timePressed)
    {
        //intended time vs curr time
        //time in accupoint            
    }
}
