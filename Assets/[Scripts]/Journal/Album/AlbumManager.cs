using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumManager : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;
    [SerializeField] private List<RawImage> pics;
    public Button bu_Capture;

    public TMP_Text journalDayTxt;
    public TMP_Text journalEntryTxt;
    public Slider compareSlider;
    public Calendar calendar;

    // Start is called before the first frame update
    void Start()
    {
        journalSO.Initialize();
        ShowPics();


        if (journalSO.GetAfterCount() > 0)
            compareSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(journalSO.GetPhotoAlbum(JournalSO.AlbumPhotoType.BEFORE,DateTime.Today).width, journalSO.GetPhotoAlbum(JournalSO.AlbumPhotoType.BEFORE, DateTime.Today).height * 2); 
    }

    public void ShowPics()
    {
        int imageCount = journalSO.GetAfterCount();
        //Debug.Log(imageCount);

        for (int i = imageCount; i > 0; i--)
        {
            //Debug.Log(imageCount);
            //Debug.Log(pics.Count);
            if (imageCount >= 8 && i == 2)
            {
                break;
            }

            pics[i - 1].texture = journalSO.GetPhotoAlbum(JournalSO.AlbumPhotoType.AFTER, DateTime.Today);
            
            //Debug.Log(i);
        }

    }

    //public void GetDayPics(int n)
    //{
    //    int imageCount = journalSO.GetAfterCount();
    //    //Debug.Log(imageCount);

    //    for (int i = imageCount; i > 0; i--)
    //    {
    //        //Debug.Log(imageCount);
    //        //Debug.Log(pics.Count);
    //        if (imageCount >= 8 && i == 2)
    //        {
    //            break;
    //        }

    //        journalSO.GetDate(i);

    //        //Debug.Log(i);
    //    }
    //}

    //public void GetText(int n)
    //{
    //    journalEntryTxt.text = journalSO.GetText(n);
    //    journalDayTxt.text = "Day " + journalSO.GetDayNum(n).ToString();
    //}

    //public void ResetCapture()
    //{
    //    journalSO.SetHasCaptured(false);
    //}
}
