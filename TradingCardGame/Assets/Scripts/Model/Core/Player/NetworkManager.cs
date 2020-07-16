using System.IO;
using System.Text;
using UnityEngine;

public class NetworkManager : INetworkManager
{
    private readonly string path = $"{Application.dataPath}/StreamingAssets/userdata.json";

    public NetworkManager() { }

    public void SendToSaveDecks(string decksJsonString)
    {
        // Запись в текстовой файл
        using (var sw = new StreamWriter(path, false, Encoding.Default))
            sw.WriteLine(decksJsonString);
    }

    public void SendToSaveData(string decksJsonString)
    {
        using (var sw = new StreamWriter(path, false, Encoding.Default))
            sw.WriteLine(decksJsonString);
    }

    public string GetUserData(string login, string password)
    {
        string jsonString = string.Empty;
        using (var sr = new StreamReader(path))
        {
            jsonString = sr.ReadToEnd();
        }

        if (jsonString == null || jsonString == string.Empty)
            throw new System.Exception("Не удалось получить данные!");

        return jsonString;
    }
}
