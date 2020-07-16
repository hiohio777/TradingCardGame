using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CommandLogger : IGameLogget
{
    private readonly string path = $"{Application.dataPath}/StreamingAssets/logger.txt";
    List<string> loggHistory = new List<string>();

    public CommandLogger(string path)
    {
        this.path = path ?? throw new ArgumentNullException(nameof(path));
        using (var sw = new StreamWriter(path, true, Encoding.Default))
            sw.WriteLine("CommandLogger: Запущен!");
    }

    public void Log(string message)
    {
        if (loggHistory.Count >= 100)
        {
            using (var sw = new StreamWriter(path, true, Encoding.Default))
                for (int i = 0; i < loggHistory.Count; i++)
                {
                    sw.WriteLine(loggHistory[i]);
                }
            loggHistory.Clear();
        }

        loggHistory.Add($"CommandLogger: {message} | time: {DateTime.Now}");
    }
}
