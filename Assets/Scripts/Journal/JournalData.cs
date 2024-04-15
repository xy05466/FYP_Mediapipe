using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JournalData
{
    private Texture2D picToLoad { get; set; }
    private Texture2D picsTaken { get; set; }
    //private List<Texture2D> beforeList { get; set; }
    //private List<Texture2D> afterList { get; set; }
    private List<int> dayNum { get; set; }
    private List<string> dayTxts { get; set; }
    private int beforeCount { get; set; }
    private int afterCount { get; set; }
    private bool hasCaptured { get; set; }
}
