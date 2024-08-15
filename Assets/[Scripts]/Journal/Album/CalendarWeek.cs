using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class CalendarWeek : MonoBehaviour
    {
        public Button[] butonDays;
        public Color colorDay = Color.black;
        public Color colorBgDay = Color.white;
        public Color colorBgHighlightDay = Color.white;

        public Action<string> OnButtonPressed;

        public void Initialize(Color colorDay, Color colorBGDay, Color colorBgHighlightDay)
        {
            this.colorDay = colorDay;
            this.colorBgDay = colorBGDay;
            this.colorBgHighlightDay = colorBgHighlightDay;


            butonDays = GetComponentsInChildren<Button>();
            foreach (var buDays in butonDays)
            {

                buDays.onClick.AddListener(() => { OnPressed(buDays.GetComponentInChildren<TMP_Text>().text); });
                buDays.GetComponentInChildren<TMP_Text>().color = colorDay;
            }
        }
        // set day text
        public void SetDays(int index, int day)
        {
            TMP_Text tmpText = butonDays[index].GetComponentInChildren<TMP_Text>();
            tmpText.text = day.ToString();
        }
        public int GetDaysCount()
        {
            return butonDays.Length;
        }

        internal void EnableDay(int index, bool enable)
        {
            TMP_Text tmpText = butonDays[index].GetComponentInChildren<TMP_Text>();

            butonDays[index].interactable = enable;

            if (enable)
                tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 1f);
            else
                tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0.5f);

        }

        internal void HighlightDay(int index, bool enable)
        {
            if (enable)
                butonDays[index].GetComponent<Image>().color = colorBgHighlightDay;
            else
                butonDays[index].GetComponent<Image>().color = colorBgDay;
        }
        internal void HighlightDayCustom(int index, Color customColor)
        {
            butonDays[index].GetComponent<Image>().color = customColor;
        }

        void OnPressed(string text)
        {
            OnButtonPressed?.Invoke(text);
        }
        public void Subscribe(Action<string> action)
        {
            OnButtonPressed += action;
        }
        public void Unubscribe(Action<string> action)
        {
            OnButtonPressed -= action;
        }
    }
}