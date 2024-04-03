using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumManager : MonoBehaviour
{
    [SerializeField] private JournalSO journalSO;
    [SerializeField] private List<RawImage> pics;

    public TMP_Text journalDayTxt;
    public TMP_Text journalEntryTxt;

    // Start is called before the first frame update
    void Start()
    {
        journalSO.Initialize();
        ShowPics();
    }

    public void ShowPics()
    {

        int imageCount = journalSO.GetPicsCount();
        Debug.Log(imageCount);

        for (int i = imageCount - 1; i >= 0; i--)
        {
            Debug.Log(imageCount);
            Debug.Log(pics.Count);
            if (i >= pics.Count)
            {
                break;
            }

            pics[i].texture = journalSO.GetPicAlbum(i);
        }

    }

    public void GetText(int n)
    {
        journalEntryTxt.text = journalSO.GetText(n);
        journalDayTxt.text = "Day " + journalSO.GetDayNum(n).ToString();
    }

}
