using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GestureSO : ScriptableObject 
{
  public string gestureName;
  public List<Vector2> fingerDistances;
  //to storee a range of values 
  //20 values range of minX MaxX minY MaxY
}
