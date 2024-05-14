using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{

    public class Day
    {
        public int dayNum;
        public Color dayColor;
        public GameObject obj;
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
            obj.GetComponent<Image>().color = newColor;
            dayColor = newColor;
        }

        public void UpdateDay(int newDayNum)
        {
            this.dayNum = newDayNum;
            if (dayColor == Color.white || dayColor == Color.green) 
            {
                obj.GetComponentInChildren<Text>().text = (dayNum + 1).ToString();
            }

            else 
            {
                obj.GetComponentInChildren<Text>().text = "";    

            }
        }

    }

    private List<Day> days = new List<Day>();

    public Transform[] weeks;

    public Text MonthAndYear;

    public DateTime currDate = DateTime.Now;




    // Start is called before the first frame update
    void Start()
    {
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }

    void UpdateCalendar(int year, int month)
    {
        DateTime temp = new DateTime(year, month ,1);
        currDate = temp;
        MonthAndYear.text = temp.ToString("MMMM") + " " + temp.Year.ToString();
        int startDay = GetMonthStartDay(year, month);
        int endDay = GetTotalNumberOfDays(year, month);

        if (days.Count == 0) 
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7;i ++)
                {
                    Day newDay;
                    int currDay = (w * 7) + i;
                    if (currDay < startDay || currDay - startDay < endDay)
                    {
                        newDay = new Day(currDay - startDay, Color.grey, weeks[w].GetChild(i).gameObject);
                    }
                    else
                    {
                        newDay = new Day(currDay - startDay, Color.white, weeks[w].GetChild(i).gameObject);
                    }
                    days.Add(newDay);
                }
            }
        }
    }

    int GetMonthStartDay(int year, int month)
    {
        DateTime temp =new DateTime(year, month, 1);
        return (int)temp.DayOfWeek;
    }

    int GetTotalNumberOfDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
