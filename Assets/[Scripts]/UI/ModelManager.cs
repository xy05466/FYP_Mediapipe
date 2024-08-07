using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
  [SerializeField] private GameObject models;

    // Start is called before the first frame update
  public void SetModelActive()
  {
    foreach (Transform child in models.transform)
    {
      if (gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text.ToLower() == child.gameObject.name.ToLower())
      {
        child.gameObject.SetActive(true);
      }
      else
      {
        child.gameObject.SetActive(false);
      }
    }
  }
}
