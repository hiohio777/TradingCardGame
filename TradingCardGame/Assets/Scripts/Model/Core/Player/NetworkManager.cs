using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class NetworkManager : INetworkManager
{
    private readonly string path = $"{Application.dataPath}/StreamingAssets/decks.json";

    public NetworkManager() { }

    public UserDataForSerialization GetPlayerData()
    {
        string jsonString = string.Empty;
        using (var sr = new StreamReader(path))
        {
            jsonString = sr.ReadToEnd();
        }

        return new UserDataForSerialization()
        {
            decksJsonString = jsonString,
        };
    }

    public void SaveDecks(string decksJsonString)
    {
        // Запись в текстовой файл
        using (var sw = new StreamWriter(path, false, Encoding.Default))
            sw.WriteLine(decksJsonString);
    }
}
