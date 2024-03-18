using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
  [SerializeField] private JournalSO journalSO;
  [SerializeField] private List<RawImage> pics;

  private void Start()
  {
    Debug.Log("start");
  }
  public void ShowPics()
  {
    int imageCount = PlayerPrefs.GetInt("imagecount");

    for (int i = 0; i < imageCount; i++)
    {
      pics[i].texture = journalSO.GetPic(pics[i].texture, i);
      pics[i].transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    ////picShow.texture = journalSO.GetPic(picShow.texture, i);
    //picShow.transform.rotation = Quaternion.Euler(0, 0, -90);
    //Debug.Log("show");
  }
}
