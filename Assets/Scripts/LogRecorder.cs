using System;
using System.IO;
using UnityEngine;

public class LogRecorder : MonoBehaviour
{
    private string logFilePath;
    private StreamWriter logWriter;

    void Awake()
    {
        // Ensure this persists across scenes
        DontDestroyOnLoad(gameObject);

        // Set file path (modify this if needed)
        string folderPath = Application.persistentDataPath + "/Logs";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logFilePath = $"{folderPath}/DebugLog_{timestamp}.txt";

        // Open StreamWriter
        logWriter = new StreamWriter(logFilePath, true);
        logWriter.AutoFlush = true;

        // Subscribe to log event
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logEntry = $"{DateTime.Now:HH:mm:ss} [{type}] {logString}";
        if (type == LogType.Exception || type == LogType.Error)
            logEntry += $"\n{stackTrace}";

        // Write to file
        logWriter.WriteLine(logEntry);
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        Application.logMessageReceived -= HandleLog;

        // Close file
        logWriter?.Close();
    }
}

