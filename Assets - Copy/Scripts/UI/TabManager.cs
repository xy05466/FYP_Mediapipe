using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
  public GameObject[] tabs;

  public void TurnOnTabs(int tab)
  {
    for (int i = 0; i < tabs.Length; i++)
    {
      tabs[i].SetActive(false);
    }
    tabs[tab - 1].SetActive(true);
  }

  // Start is called before the first frame update
  void Start()
  {
            
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
