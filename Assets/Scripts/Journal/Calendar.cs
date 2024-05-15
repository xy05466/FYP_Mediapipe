using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Calendar : MonoBehaviour
{
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
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);

        journalSO.Initialize();

        if (journalSO.GetAfterCount() > 0)
        {
            compareSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(journalSO.GetBeforeAlbum(1).width, journalSO.GetBeforeAlbum(1).height * 2);
        }
    }

    public void GetDayPics()
    {
        datePressed = EventSystem.current.currentSelectedGameObject;

        String temp = new DateTime(currDate.Year, currDate.Month, int.Parse(datePressed.GetComponentInChildren<TMP_Text>().text)).ToString("dd MMMM yyyy");

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

    public void LoadSliderImages()
    {
        string fileLocation;
        if (Application.platform == RuntimePlatform.Android)
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

        if (Application.platform == RuntimePlatform.Android)
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

    void UpdateCalendar(int year, int month)
    {
        DateTime temp = new DateTime(year, month ,1);
        currDate = temp;
        MonthAndYear.text = temp.ToString("MMMM") + " " + temp.Year.ToString();
        int startDay = GetMonthStartDay(year, month);
        int endDay = GetTotalNumberOfDays(year, month);
        int a;
        int n = 0;
        int prevDates = 0;
        int nextDates = 0;

        if (days.Count == 0) 
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7;i ++)
                {
                    Day newDay;
                    int currDay = (w * 7) + i;
                    if (currDay < startDay || currDay - startDay >= endDay)
                    {
                        newDay = new Day(currDay - startDay, unavailableColor, weeks[w].GetChild(i).gameObject);
                    }
                    else
                    {
                        newDay = new Day(currDay - startDay, availableColor, weeks[w].GetChild(i).gameObject);
                    }
                    days.Add(newDay);
                }
            }
            setPrevNextDates(startDay,endDay, prevDates, nextDates, year, month);
        }

        else
        {
            setPrevNextDates(startDay, endDay, prevDates, nextDates, year, month);
        }
        if (DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            days[(DateTime.Now.Day - 1) + startDay].UpdateColor(Color.white);
            days[(DateTime.Now.Day - 1) + startDay].UpdateButtonColor(availableColor);
        }
    }

    void setPrevNextDates(int startDay, int endDay , int prevDates, int nextDates, int year, int month)
    {
        for (int i = 0; i < 42; i++)
        {
            if (i < startDay)
            {
                prevDates++;

            }
            if (i - startDay >= endDay)
            {
                nextDates++;
            }
            else
            {
                days[i].UpdateColor(availableColor);
            }

            if (days.Count != 0)
            {
                days[i].UpdateDay(i - startDay);
            }
        }
        int a = GetTotalNumberOfDays(year, month + 1);
        int n = GetTotalNumberOfDays(year, month - 1) - prevDates;

        for (int i = 0; i < nextDates; i++)
        {
            days[endDay + startDay].UpdateDay(GetMonthDay(year, month + 1, i + 1) - 1);
            days[endDay + startDay].UpdateColor(unavailableColor);
            endDay++;
        }

        for (int i = 0; i < prevDates; i++)
        {
            days[i].UpdateDay(GetMonthDay(year, month - 1, n));
            days[i].UpdateColor(unavailableColor);
            n++;
        }
    }

    int GetMonthStartDay(int year, int month)
    {
        DateTime temp =new DateTime(year, month, 1);
        return (int)temp.DayOfWeek;
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
        {
            currDate = currDate.AddMonths(-1);
        }
        else 
        {
            currDate = currDate.AddMonths(1);
        }
        UpdateCalendar(currDate.Year, currDate.Month);
    }
}
