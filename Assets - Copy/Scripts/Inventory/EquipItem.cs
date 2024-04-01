using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem : MonoBehaviour
{
  //[SerializeField] private Image itemImage;
  //[SerializeField] private Sprite itemSprite;
  //[SerializeField] private Sprite defaultHeadSprite;
  //[SerializeField] private Sprite defaultBodySprite;
  //[SerializeField] private Sprite defaultTailSprite;
  [SerializeField] private Image equipHeadImage;
  [SerializeField] private Image equipBodyImage;
  [SerializeField] private Image equipTailImage;
  [SerializeField] private InventorySO inventorySO;
  [SerializeField] private ItemSO item;

  public void SetHeadData()
  {
    equipHeadImage.sprite = item.sprite;
    SetImageAlpha(equipHeadImage);
    inventorySO.ChangeHeadItem(item);
  }

  public void SetBodyData()
  {
    equipBodyImage.sprite = item.sprite;
    SetImageAlpha(equipBodyImage);
    inventorySO.ChangeBodyItem(item);
  }

  private void SetImageAlpha(Image image)
  {
    Color tmp = image.color;
    tmp.a = 255f;
    image.color = tmp;
  }

  //// Start is called before the first frame update
    void Start()
    {
    //if (inventorySO.GetHeadItem() == null)
    //{
    //  equipHeadImage.sprite = defaultHeadSprite;
    //}
    //if (inventorySO.GetBodyItem() == null)
    //{
    //  equipBodyImage.sprite = defaultBodySprite;
    //}
    //if (inventorySO.GetTailItem() == null)
    //{
    //  equipTailImage.sprite = defaultTailSprite;
    //}  
    if (inventorySO.GetHeadItem() != null)
    {
      equipHeadImage.sprite = inventorySO.GetHeadItem();
      SetImageAlpha(equipHeadImage);
    }

     
    if (inventorySO.GetBodyItem() != null)
    {
      equipBodyImage.sprite = inventorySO.GetBodyItem();
      SetImageAlpha(equipBodyImage);
    }
      
    }

  //  // Update is called once per frame
  //  void Update()
  //  {
        
  //  }
}
