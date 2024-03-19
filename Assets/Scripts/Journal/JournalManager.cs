using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class JournalManager : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;
    [SerializeField] private List<RawImage> pics;

    public RawImage picResult;
    public TMP_InputField journalEntry;
    public TMP_Text journalDayTxt;

    private void Start()
    {
        journalSO.Initialize();
    }
    

    public void ShowPic()
    {
        //PlayerPrefs.SetInt("imageCount",0);
        int picNum = journalSO.GetPicsCount();
        picResult.texture = journalSO.GetPicAlbum(picNum - 1);
    }

    public void SetText()
    {
        int jNum = journalSO.GetPicsCount();
        journalSO.SetText(journalEntry.text, jNum);
    }

  
    //public void SetDayOnePic()
    //{
    //    //picTaken = (Texture2D)background.texture;
    //    StartCoroutine(journalSO.TakePic(background.texture));
    //    imageCount++;
    //    PlayerPrefs.SetInt("imageCount", imageCount);
    //}

}
