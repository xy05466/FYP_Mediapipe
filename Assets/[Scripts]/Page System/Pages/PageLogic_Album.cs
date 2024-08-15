using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class PageLogic_Album : PageLogic
    {
        public Calendar calendar;
        [SerializeField] private JournalData journalSO;
        private DateTime currDate = DateTime.Now;
        private DateTime selectedDate = DateTime.Now;
        public Button bu_seeComparison;
        [Header("Comparision Images")]
        public Texture textureInitial;
        public RawImage beforeImage;
        public RawImage afterImage;
        [Header("Comparision Sliders")]
        public Slider compareSlider;
        public GameObject sliderBG;
        public GameObject sliderFill;
        public TMP_Text dateText;
        public TMP_Text dayText;
        public override void Initialize()
        {
            bu_seeComparison.onClick.AddListener(LoadSliderImages);
            calendar.SubscribeDayPressed(OnDayClicked);
        }

        public override void StartPage()
        {

            if (journalSO.GetCount(JournalData.AlbumPhotoType.AFTER) > 0)
                compareSlider.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, DateTime.Today).width,
                    journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, DateTime.Today).height * 2);

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                beforeImage.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.5f, 1f);
                afterImage.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.5f, 1f);
            }
        }

        public void LoadSliderImages()
        {
            sliderFill.GetComponent<Image>().sprite = Sprite.Create((Texture2D)beforeImage.texture, new Rect(0, 0, afterImage.texture.width, afterImage.texture.height), new Vector2(0.5f, 0.0f), 100.0f);
            sliderBG.GetComponent<Image>().sprite = Sprite.Create((Texture2D)afterImage.texture, new Rect(0, 0, afterImage.texture.width, afterImage.texture.height), new Vector2(0.5f, 0.0f), 100.0f);
            dateText.text = string.Format("{0} {1} {2}", selectedDate.Day, selectedDate.ToString("MMMM"), selectedDate.Year);
            dayText.text = string.Format("{0} {1}", selectedDate.Day, selectedDate.ToString("MMMM"));
        }

        private void OnDayClicked(DateTime dateTime)
        {
            beforeImage.texture = journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, selectedDate);
            if (beforeImage.texture == null)
                beforeImage.texture = textureInitial;

            afterImage.texture = journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.AFTER, selectedDate);
            if (afterImage.texture == null)
                afterImage.texture = textureInitial;
        }
    }
}