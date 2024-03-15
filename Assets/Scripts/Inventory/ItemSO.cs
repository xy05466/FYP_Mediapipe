using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ItemSO : ScriptableObject
{
  [field:SerializeField]
  public bool unlocked { get; set; }

  [field:SerializeField]
  public string name { get; set; }

  [field:SerializeField]
  public int id { get; set; }

  [field:SerializeField]
  public Sprite sprite { get; set; }


}
