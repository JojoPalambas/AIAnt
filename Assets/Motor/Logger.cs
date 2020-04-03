using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Logger
{
    private enum LogType
    {
        INFO,
        WARNING,
        ERROR
    }

    private class MessageDescriptor
    {
        private string message;
        private LogType type;

        private bool active;

        public MessageDescriptor(string message, LogType type)
        {
            this.message = message;
            this.type = type;
        }

        public string ToString()
        {
            string prettyType = "TYPE";
            switch (type)
            {
                case LogType.INFO:
                    prettyType = "---- INFO";
                    break;
                case LogType.WARNING:
                    prettyType = "- WARNING";
                    break;
                case LogType.ERROR:
                    prettyType = "--- ERROR";
                    break;
                default:
                    prettyType = "";
                    break;
            }

            return "--" + prettyType + " | " + message;
        }
    }

    private static bool shouldLog = true;
    private static bool hasBeenInit = false;

    private static List<MessageDescriptor> buffer = new List<MessageDescriptor>();
    private static int maxBufferSize = 1000;

    public static void Init()
    {
        if (!shouldLog)
            return;
        if (hasBeenInit)
            return;

        hasBeenInit = true;

        System.IO.StreamWriter file = new System.IO.StreamWriter(@"Logs.txt");
        file.WriteLine("\n\n\n========== NEW PROGRAM START - " + System.DateTime.Now.ToString());
        file.Close();
    }

    public static void Info(object obj)
    {
        if (!shouldLog)
            return;

        Init();

        buffer.Add(new MessageDescriptor(obj.ToString(), LogType.INFO));

        if (buffer.Count >= maxBufferSize)
        {
            Flush();
        }
    }

    public static void Warning(object obj)
    {
        if (!shouldLog)
            return;

        Init();

        buffer.Add(new MessageDescriptor(obj.ToString(), LogType.WARNING));

        if (buffer.Count >= maxBufferSize)
        {
            Flush();
        }
    }

    public static void Error(object obj)
    {
        if (!shouldLog)
            return;

        Init();

        buffer.Add(new MessageDescriptor(obj.ToString(), LogType.ERROR));

        if (buffer.Count >= maxBufferSize)
        {
            Flush();
        }
    }

    public static void Flush()
    {
        if (!shouldLog)
            return;
        if (!hasBeenInit)
            return;

        Debug.Log("Logger flush");

        string bufferAsString = "";
        foreach (MessageDescriptor message in buffer)
        {
            bufferAsString += message.ToString() + "\n";
        }

        System.IO.StreamWriter file = System.IO.File.AppendText(@"Logs.txt");
        file.WriteLine(bufferAsString);
        file.Close();

        buffer = new List<MessageDescriptor>();
    }
}
