using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public GameObject flowerImage;
    public Animator transition;
    public MusicSystem musicSystem;

    public float countdownTime = 1f;
    public void StartDisableCountdown()
    {
        transition.GetComponent<Animator>().enabled = true;
        StartCoroutine(DisableCountdown());
    }

    IEnumerator DisableCountdown()
    {
        yield return new WaitForSeconds(countdownTime);
        
        flowerImage.SetActive(false);
        musicSystem.StartGame();
    }
}
