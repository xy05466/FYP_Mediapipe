using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
  [SerializeField]
  private ItemSO headItem;
  private ItemSO tailItem;
  private ItemSO bodyItem;

  [field: SerializeField]

  //public void Initialize()
  //{
  //  inventoryItems = new List<InventoryItem>();
  //  for (int i = 0; i < size; i++)
  //  {
  //    inventoryItems.Add(new InventoryItem());
  //  }
  //}

  public void ChangeHeadItem(ItemSO itemSO)
  {
    //Debug.Log(headItem.name);
    headItem = itemSO;
    Debug.Log(headItem.name);
  }

  public void ChangeBodyItem(ItemSO itemSO)
  {
    bodyItem = itemSO;
    Debug.Log(bodyItem.name);
  }

  public Sprite GetHeadItem()
  {
    if (headItem == null)
    {
      return null;
    }
    return headItem.sprite;  
  }

  public Sprite GetBodyItem()
  {
    if (bodyItem == null)
    {
      return null;
    }
    return bodyItem.sprite;
  }

  public Sprite GetTailItem()
  {
    if (tailItem == null)
    {
      return null;
    }
    return tailItem.sprite;
  }
}

//[Serializable]
//public struct InventoryItem
//{
//  public int quantity;
//  public ItemSO item;

//  public static InventoryItem GetEmptyItem()
//    => new InventoryItem
//    {

//    };
//}
