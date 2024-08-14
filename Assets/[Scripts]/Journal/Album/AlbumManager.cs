using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
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
}