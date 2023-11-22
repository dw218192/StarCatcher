using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogHelper : MonoBehaviour
{
    public Text logText;
    public Text scoreText;
#if UNITY_EDITOR
    bool debugMode = true;
#else
    bool debugMode = false;
#endif

    static public LogHelper instance = null;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else {
            instance = this;
            if(debugMode)
                Application.logMessageReceived += HandleLog;
        }
    }

    uint qsize = 15;  // number of messages to keep
    Queue myLogQueue = new();

    private void OnDestroy()
    {
        if (debugMode)
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

    public void SetScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
}