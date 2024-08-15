using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class PageLogic_Album : PageLogic
    {
        [SerializeField] private JournalData journalSO;
        [SerializeField] private List<RawImage> pics;
        public Button bu_Capture;

        public TMP_Text journalDayTxt;
        public TMP_Text journalEntryTxt;
        public Slider compareSlider;
        public Calendar calendar;

        public override void Initialize()
        {
        }
        public override void StartPage()
        {
            journalSO.Initialize();
            ShowPics();


            if (journalSO.GetAfterCount() > 0)
                compareSlider.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, DateTime.Today).width,
                    journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, DateTime.Today).height * 2);
        }
        public override void RunLogic(List<HandPointAnnotationObject> handpointsObj_1, List<HandPointAnnotationObject> handpointsObj_2, List<GameObject> facepointsObj)
        {
        }

        public override void EndPage()
        {
        }

        public override bool IsPageDone()
        {
            return false;
        }

        public override void OnHandPointCollideFacePoint(int hand, int handPointIndex, GameObject handPointObj, int facePointIndex, GameObject facePointObject)
        {
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

                pics[i - 1].texture = journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.AFTER, DateTime.Today);

            }
        }
    }
}