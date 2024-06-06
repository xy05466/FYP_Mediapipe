using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Calendar : MonoBehaviour
{
    public List<CalendarWeek> calendarWeeks = new List<CalendarWeek>();
    [Header("Styling")]
    public Color colorDay = Color.black;
    public Color colorBackgroundDay = Color.white;
    public Color colorBackgroundCurrentDay = Color.yellow;

    [Header("Buttons")]
    public Button bu_next;
    public Button bu_prev;

    void InitializeButtons()
    {
        bu_next.onClick.AddListener(() => { SwitchMonth(1); });
        bu_prev.onClick.AddListener(() => { SwitchMonth(-1); });
    }

    public class Day
    {
        public int dayNum;
        public Color dayColor;
        public GameObject obj;
        
        Color availableColor = new Color(0.3647059f, 0.2313726f, 0.04705883f, 1.0f);
        Color unavailableColor = new Color(0.6509804f, 0.5568628f, 0.454902f, 1.0f);

        public Day(int dayNum, Color dayColor, GameObject obj)
        {
            this.dayNum = dayNum;
            this.dayColor = dayColor;
            this.obj = obj;
            UpdateColor(dayColor);
            UpdateDay(dayNum);
        }

        public void UpdateColor(Color newColor)
        {
            obj.GetComponentInChildren<TMP_Text>().color = newColor;
            dayColor = newColor;
        }

        public void UpdateButtonColor(Color newColor)
        {
            obj.GetComponent<Image>().color = newColor;
            dayColor = newColor;
        }

        public void UpdateDay(int newDayNum)
        {
            this.dayNum = newDayNum;
            if (dayColor == availableColor || dayColor == Color.green) 
            {
                obj.GetComponentInChildren<TMP_Text>().text = (dayNum + 1).ToString();
                //obj.GetComponentInChildren<TMP_Text>().color = availableColor;
            }

            else 
            {
                obj.GetComponentInChildren<TMP_Text>().text = (dayNum + 1).ToString();
                obj.GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
                //obj.GetComponentInChildren<TMP_Text>().color = unavailableColor;
            }
        }
    }

    private List<Day> days = new List<Day>();

    private int picIndex;

    private GameObject datePressed;

    public Transform[] weeks;

    public TMP_Text MonthAndYear;

    public DateTime currDate = DateTime.Now;
    public DateTime selectedDate = DateTime.Now;

    [SerializeField] private JournalSO journalSO;
    [SerializeField] private List<RawImage> pics;

    public TMP_Text journalDayTxt;
    public TMP_Text journalEntryTxt;
    public UnityEngine.UI.Slider compareSlider;
    public RawImage beforeImage;
    public RawImage afterImage;

    public GameObject sliderBG;
    public GameObject sliderFill;
    public TMP_Text dateText;
    public TMP_Text dayText;

    Color availableColor = new Color(0.3647059f, 0.2313726f, 0.04705883f, 1.0f);
    Color unavailableColor = new Color(0.6509804f, 0.5568628f, 0.454902f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        CalendarWeek[] weeks = GetComponentsInChildren<CalendarWeek>();
        foreach (CalendarWeek week in weeks) {
            week.Initialize(colorDay, colorBackgroundDay, colorBackgroundCurrentDay);
            calendarWeeks.Add(week);
            week.Subscribe(OnDayPressed);
        }
        UpdateCalendar(DateTime.Now);

        InitializeButtons();

        journalSO.Initialize();

        if (journalSO.GetAfterCount() > 0)
        {
            compareSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(journalSO.GetBeforeAlbum(1).width, journalSO.GetBeforeAlbum(1).height * 2);
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            beforeImage.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.5f, 1f);
            afterImage.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.5f, 1f);
        }
    }

    public void GetDayPics()
    {

    }

    public void LoadSliderImages()
    {
        string fileLocation;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fileLocation = Application.persistentDataPath + "/BeforeAlbum/CameraScreenshot" + picIndex + ".png";
        }
        else
        {
            fileLocation = Application.dataPath + "/BeforeAlbum/CameraScreenshot" + picIndex + ".png";
        }
        FileStream beforeFs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        byte[] beforeImageData = new byte[beforeFs.Length];
        beforeFs.Read(beforeImageData, 0, (int)beforeFs.Length);
        Texture2D beforePic = new Texture2D(1080, 1920, TextureFormat.RGBA32, false);
        beforePic.LoadImage(beforeImageData);
        Sprite beforeSprite = Sprite.Create(beforePic, new Rect(0, 0, beforePic.width, beforePic.height), new Vector2(0.5f, 0.0f), 100.0f);

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fileLocation = Application.persistentDataPath + "/AfterAlbum/CameraScreenshot" +picIndex + ".png";
        }
        else
        {
            fileLocation = Application.dataPath + "/AfterAlbum/CameraScreenshot" + picIndex + ".png";
        }
        FileStream afterFs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);

        byte[] afterImageData = new byte[afterFs.Length];
        afterFs.Read(afterImageData, 0, (int)afterFs.Length);
        Texture2D afterPic = new Texture2D(1080, 1920, TextureFormat.RGBA32, false);
        afterPic.LoadImage(afterImageData);
        Sprite afterSprite = Sprite.Create(afterPic, new Rect(0, 0, afterPic.width, afterPic.height), new Vector2(0.5f, 0.0f), 100.0f);


        sliderFill.GetComponent<Image>().sprite = beforeSprite;

        sliderBG.GetComponent<Image>().sprite = afterSprite;

        dateText.text = journalSO.date[picIndex - 1];
        dayText.text = journalSO.day[picIndex - 1];
    }

    public void GetText(int n)
    {
        journalEntryTxt.text = journalSO.GetText(n);
        journalDayTxt.text = "Day " + journalSO.GetDayNum(n).ToString();
    }

    public void ResetCapture()
    {
        journalSO.SetHasCaptured(false);
    }

   

    //void setPrevNextDates(int startDay, int endDay , int prevDates, int nextDates, int year, int month)
    //{
    //    for (int i = 0; i < 42; i++)
    //    {
    //        if (i < startDay)
    //        {
    //            prevDates++;

    //        }
    //        if (i - startDay >= endDay)
    //        {
    //            nextDates++;
    //        }
    //        else
    //        {
    //            days[i].UpdateColor(availableColor);
    //        }

    //        if (days.Count != 0)
    //        {
    //            days[i].UpdateDay(i - startDay);
    //        }
    //    }
    //    int a = GetTotalNumberOfDays(year, month + 1);
    //    int n = GetTotalNumberOfDays(year, month - 1) - prevDates;

    //    for (int i = 0; i < nextDates; i++)
    //    {
    //        days[endDay + startDay].UpdateDay(GetMonthDay(year, month + 1, i + 1) - 1);
    //        days[endDay + startDay].UpdateColor(unavailableColor);
    //        endDay++;
    //    }

    //    for (int i = 0; i < prevDates; i++)
    //    {
    //        days[i].UpdateDay(GetMonthDay(year, month - 1, n));
    //        days[i].UpdateColor(unavailableColor);
    //        n++;
    //    }
    //}

    //int GetMonthStartDay(int year, int month)
    //{
    //    DateTime temp =new DateTime(year, month, 1);
    //    return (int)temp.DayOfWeek;
    //}

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
                DateTime DateI =focusDate.AddDays(dayAdd);
                week.SetDays(i, DateI.Day);
                week.HighlightDay(i, false);

                if (DateI.Year == selectedDate.Year && DateI.Month == selectedDate.Month)
                {
                    week.EnableDay(i,true);
                }
                else
                {
                    week.EnableDay(i, false);
                }

                if (DateI.Year == currDate.Year && DateI.Month == currDate.Month && DateI.Day == currDate.Day)
                    week.HighlightDay(i, true);

                dayAdd++;
            }
        }

        // get the first week day of week
        // get end of the week
    }

    void OnDayPressed(string text)
    {
        Debug.Log(text);


        String temp = new DateTime(selectedDate.Year, selectedDate.Month, int.Parse(text)).ToString("dd MMMM yyyy");
        Debug.Log(temp);
        int imageCount = journalSO.GetAfterCount();

        for (int i = imageCount; i > 0; i--)
        {
            if (journalSO.GetDate(i - 1) == temp)
            {
                beforeImage.texture = journalSO.GetBeforeAlbum(i);
                afterImage.texture = journalSO.GetAfterAlbum(i);
                picIndex = i;
            }
        }
    }
}
