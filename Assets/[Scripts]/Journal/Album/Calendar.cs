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

        [Header("Buttons")]
        public Button bu_next;
        public Button bu_prev;

        private CalendarWeek[] weeks;

        public TMP_Text MonthAndYear;

        public DateTime currDate = DateTime.Now;
        public DateTime selectedDate = DateTime.Now;




        private Action<DateTime> OnClickDate;

        void InitializeButtons()
        {
            bu_next.onClick.AddListener(() => { SwitchMonth(1); });
            bu_prev.onClick.AddListener(() => { SwitchMonth(-1); });
        }

        // Start is called before the first frame update
        void Start()
        {
            InitializeButtons();

            CalendarWeek[] weeks = GetComponentsInChildren<CalendarWeek>(true);
            foreach (CalendarWeek week in weeks)
            {
                week.Initialize(colorDay, colorBackgroundDay, colorBackgroundCurrentDay);
                calendarWeeks.Add(week);
                week.Subscribe(OnDayPressed);
            }
            // update calendar
            UpdateCalendar(DateTime.Now);
            OnDayPressed(DateTime.Today.Day.ToString());
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
        private void OnDayPressed(string text)
        {
            int day = int.Parse(text);
            selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, day);
            OnClickDate?.Invoke(new DateTime(selectedDate.Year, selectedDate.Month, day));

        }
        // EVENTS
        public void SubscribeDayPressed(Action<DateTime> action) => OnClickDate += action;
        public void UnubscribeDayPressed(Action<DateTime> action)=> OnClickDate -= action;
    }
}