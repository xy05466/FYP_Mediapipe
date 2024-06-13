using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
  [SerializeField] RectTransform contentPanel;


  [SerializeField] int inventorySize = 10;

  //public void InitializeInventoryUI(int inventorySize)
  //{
  //  for (int i = 0; i < inventorySize; i++)
  //  {
  //    InventoryItem item = Instantiate(inventoryItem, Vector3.zero, Quaternion.identity);
  //    item.transform.SetParent(contentPanel);
  //    inventoryItems.Add(item);
  //  }
  //}

  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  //  // Start is called before the first frame update
  //  void Start()
  //  {
  //  for (int i = 0; i < inventorySize; i++)
  //  {
  //    InventoryItem item = Instantiate(inventoryItem, Vector3.zero, Quaternion.identity);
  //    item.transform.SetParent(contentPanel);
  //    inventoryItems.Add(item);
  //  }
  //}
}
