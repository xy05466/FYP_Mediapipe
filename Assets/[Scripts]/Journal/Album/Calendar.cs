using System;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace SDM_FythmPipe
{
    public class Calendar : MonoBehaviour
    {
        public List<CalendarWeek> calendarWeeks = new List<CalendarWeek>();
        [Header("Styling")]
        public Color colorDay = Color.black;
        public Color colorBackgroundDay = Color.white;
        public Color colorBackgroundCurrentDay = Color.yellow;
        //public Color colorSelected = Color.green;

        [Header("Buttons")]
        public Button bu_next;
        public Button bu_prev;
        public Button bu_seeComparison;
        [SerializeField] private JournalData journalSO;



        //public class Day
        //{
        //    public int dayNum;
        //    public Color dayColor;
        //    public GameObject obj;

        //    Color availableColor = new Color(0.3647059f, 0.2313726f, 0.04705883f, 1.0f);
        //    Color unavailableColor = new Color(0.6509804f, 0.5568628f, 0.454902f, 1.0f);

        //    public Day(int dayNum, Color dayColor, GameObject obj)
        //    {
        //        this.dayNum = dayNum;
        //        this.dayColor = dayColor;
        //        this.obj = obj;
        //        UpdateColor(dayColor);
        //        UpdateDay(dayNum);
        //    }

        //    public void UpdateColor(Color newColor)
        //    {
        //        obj.GetComponentInChildren<TMP_Text>().color = newColor;
        //        dayColor = newColor;
        //    }

        //    public void UpdateButtonColor(Color newColor)
        //    {
        //        obj.GetComponent<Image>().color = newColor;
        //        dayColor = newColor;
        //    }

        //    public void UpdateDay(int newDayNum)
        //    {
        //        this.dayNum = newDayNum;
        //        if (dayColor == availableColor || dayColor == Color.green) 
        //        {
        //            obj.GetComponentInChildren<TMP_Text>().text = (dayNum + 1).ToString();
        //            //obj.GetComponentInChildren<TMP_Text>().color = availableColor;
        //        }

        //        else 
        //        {
        //            obj.GetComponentInChildren<TMP_Text>().text = (dayNum + 1).ToString();
        //            obj.GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
        //            //obj.GetComponentInChildren<TMP_Text>().color = unavailableColor;
        //        }
        //    }
        //}

        //private List<Day> days = new List<Day>();

        //private int picIndex;

        //private GameObject datePressed;

        public CalendarWeek[] weeks;

        public TMP_Text MonthAndYear;

        public DateTime currDate = DateTime.Now;
        public DateTime selectedDate = DateTime.Now;

        [SerializeField] private List<RawImage> pics;

        [Header("Text Infos (NOT USED)")]
        public TMP_Text journalDayTxt;
        public TMP_Text journalEntryTxt;
        [Header("Comparision Images")]
        public Texture textureInitial;
        public RawImage beforeImage;
        public RawImage afterImage;
        [Header("Comparision Sliders")]
        public UnityEngine.UI.Slider compareSlider;
        public GameObject sliderBG;
        public GameObject sliderFill;
        public TMP_Text dateText;
        public TMP_Text dayText;

        Color availableColor = new Color(0.3647059f, 0.2313726f, 0.04705883f, 1.0f);
        Color unavailableColor = new Color(0.6509804f, 0.5568628f, 0.454902f, 1.0f);

        void InitializeButtons()
        {
            bu_next.onClick.AddListener(() => { SwitchMonth(1); });
            bu_prev.onClick.AddListener(() => { SwitchMonth(-1); });
            bu_seeComparison.onClick.AddListener(LoadSliderImages);
        }

        // Start is called before the first frame update
        void Start()
        {
            InitializeButtons();
            journalSO.Initialize();

            CalendarWeek[] weeks = GetComponentsInChildren<CalendarWeek>();
            foreach (CalendarWeek week in weeks)
            {
                week.Initialize(colorDay, colorBackgroundDay, colorBackgroundCurrentDay);
                calendarWeeks.Add(week);
                week.Subscribe(OnDayPressed);
            }
            // update calendar
            UpdateCalendar(DateTime.Now);
            OnDayPressed(DateTime.Today.Day.ToString());

            // resizing slider based on photo
            if (journalSO.GetAfterCount() > 0)
                compareSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, DateTime.Today).width, journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, DateTime.Today).height * 2);

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

        // Not used
        public void GetText(int n)
        {
            journalEntryTxt.text = journalSO.GetText(n);
            journalDayTxt.text = "Day " + journalSO.GetDayNum(n).ToString();
        }

        public void ResetCapture()
        {
            journalSO.SetHasCapturedBefore(false);
        }
        int GetMonthDay(int year, int month, int day)
        {
            DateTime temp = new DateTime(year, month, day);
            return (int)temp.Day;
        }

        int GetTotalNumberOfDays(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public void SwitchMonth(int direction)
        {
            if (direction < 0)
                selectedDate = selectedDate.AddMonths(-1);
            else
                selectedDate = selectedDate.AddMonths(1);

            UpdateCalendar(selectedDate);
        }


        // Update calendar
        private void UpdateCalendar(DateTime startDate)
        {
            MonthAndYear.text = startDate.ToString("MMMM") + " " + startDate.Year.ToString();

            DateTime startMonth = new DateTime(startDate.Year, startDate.Month, 1);
            DateTime focusDate = startMonth.AddDays(-(int)startMonth.DayOfWeek);

            int dayAdd = 0;
            foreach (var week in calendarWeeks)
            {
                for (int i = 0; i < week.GetDaysCount(); i++)
                {
                    DateTime DateI = focusDate.AddDays(dayAdd);
                    week.SetDays(i, DateI.Day);
                    week.HighlightDay(i, false);

                    // check if year and mont hsame
                    if (DateI.Year == selectedDate.Year && DateI.Month == selectedDate.Month)
                    {
                        week.EnableDay(i, true);

                    }
                    else
                    {
                        week.EnableDay(i, false);
                    }

                    // highlight day if it is current date
                    if (DateI.Year == currDate.Year && DateI.Month == currDate.Month && DateI.Day == currDate.Day)
                        week.HighlightDay(i, true);

                    dayAdd++;
                }
            }

            // get the first week day of week
            // get end of the week
        }

        // when day is pressed, text returns the day text
        void OnDayPressed(string text)
        {
            String temp = new DateTime(selectedDate.Year, selectedDate.Month, int.Parse(text)).ToString("dd MMMM yyyy");
            int day = int.Parse(text);
            Debug.Log(day);
            selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, day);
            //int imageCount = journalSO.GetAfterCount();
            beforeImage.texture = journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.BEFORE, selectedDate);
            if (beforeImage.texture == null)
                beforeImage.texture = textureInitial;

            afterImage.texture = journalSO.GetPhotoAlbum(JournalData.AlbumPhotoType.AFTER, selectedDate);
            if (afterImage.texture == null)
                afterImage.texture = textureInitial;
            //for (int i = imageCount; i > 0; i--)
            //{
            //    Debug.Log(journalSO.GetDate(i - 1) + " " + temp);
            //    if (journalSO.GetDate(i - 1) == temp)
            //    {

            //        picIndex = i;
            //    }
            //}
        }
    }
}