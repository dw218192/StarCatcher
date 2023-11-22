using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogHelper : MonoBehaviour
{
    public Text logText;

    uint qsize = 15;  // number of messages to keep
    Queue myLogQueue = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.logMessageReceived += HandleLog;
    }
    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();

        if (logText != null) {
            if (type == LogType.Exception)
                logText.color = Color.red;
            else if (type == LogType.Error)
                logText.color = Color.yellow;
            else
                logText.color = Color.white;
            logText.text = string.Join("\n", myLogQueue.ToArray());
        }

    }
}