using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumManager : MonoBehaviour
{
    [SerializeField] private JournalData journalSO;
    [SerializeField] private List<RawImage> pics;
    public Button bu_Capture;

    public TMP_Text journalDayTxt;
    public TMP_Text journalEntryTxt;
    public Slider compareSlider;
    public Calendar calendar;

}
