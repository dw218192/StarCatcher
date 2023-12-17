using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro

public class DebugLogger : MonoBehaviour
{
    public TextMeshPro logText;
    uint qsize = 15;  // number of messages to keep
    Queue myLogQueue = new();

    void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }
    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error) {
            myLogQueue.Enqueue("[" + type + "] : " + logString);
            myLogQueue.Enqueue(stackTrace);
        }
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
        logText.text = string.Join("\n", myLogQueue.ToArray());
    }
}