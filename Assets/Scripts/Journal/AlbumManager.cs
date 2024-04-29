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
        Debug.Log("AlbumBeforeDebug" + journalSO.GetBeforeCount().ToString());
        Debug.Log("AlbumAfterDebug" + journalSO.GetAfterCount().ToString());
        Debug.Log("AlbumDebug" + journalSO.GetHasCaptured());
        ShowPics();
    }

    public void ShowPics()
    {

        int imageCount = journalSO.GetAfterCount();
        Debug.Log(imageCount);

        for (int i = imageCount; i > 0; i--)
        {
            Debug.Log(imageCount);
            Debug.Log(pics.Count);
            if (i >= pics.Count)
            {
                break;
            }

            pics[i - 1].texture = journalSO.GetAfterAlbum(i);
        }

    }

    public void GetText(int n)
    {
        journalEntryTxt.text = journalSO.GetText(n);
        journalDayTxt.text = "Day " + journalSO.GetDayNum(n).ToString();
    }

}
