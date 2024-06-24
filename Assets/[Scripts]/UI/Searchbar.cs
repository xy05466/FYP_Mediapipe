using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchBar : MonoBehaviour
{
  [SerializeField] private GameObject canvas;
  private GameObject contentHolder;
  private GameObject[] products;
  [SerializeField] private GameObject searchBar;
  private int totalProducts;


    // Start is called before the first frame update
    void Start()
    {
      //totalProducts = contentHolder.transform.childCount;    
      //products = new GameObject[totalProducts];

      //for (int i = 0; i < totalProducts; i++)
      //{
      //  products[i] = contentHolder.transform.GetChild(i).gameObject;
      //}
    }

    public void Search()
    {
    foreach (Transform child in canvas.transform)
    {
      if (child.gameObject.activeSelf)
      {
        totalProducts = child.GetChild(1).childCount;
        contentHolder = child.GetChild(1).gameObject;
      }
    }

    products = new GameObject[totalProducts];

    for (int i = 0; i < totalProducts; i++)
    {
      products[i] = contentHolder.transform.GetChild(i).gameObject;
      Debug.Log(contentHolder.transform.GetChild(i).gameObject.name);
    }

    string searchText = searchBar.GetComponent<TMP_InputField>().text;
    Debug.Log(searchText);
      int searchTxtLen = searchText.Length;
      int searchedProducts = 0;
      foreach(GameObject pro in products)
      {
        searchedProducts++;
        Debug.Log(pro.transform.GetChild(2).name);
        if (pro.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text.Length >= searchTxtLen)
        {
          if (searchText.ToLower() == pro.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text.Substring(0, searchTxtLen).ToLower())
          {
            pro.SetActive(true);
          }
          else
          {
            pro.SetActive(false);
          }
        }
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
