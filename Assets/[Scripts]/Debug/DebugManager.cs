using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;
    public int maxCount = 10;
    public TMP_Text tmp_log;
    public Queue<string> logs;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    private void Start()
    {

    }

    public void Log(string log)
    {
        logs.Enqueue(log);
        foreach (var logItem in logs) {
            tmp_log.text+=logItem+"\n";
        }
        if (logs.Count >= maxCount)
            logs.Dequeue();
    }
}
