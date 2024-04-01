using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class DialougeListSO : ScriptableObject
{
    public TextAsset textAssetData;
    public int numHeaders;
    [System.Serializable]
    public class Dialouge
    {
        public int id;
        public string desc;
        public string dialougeText;

    }
    [System.Serializable]
    public class DialougeList
    {
        public Dialouge[] dialouge;
    }

    public DialougeList dialougeList = new DialougeList();
}
