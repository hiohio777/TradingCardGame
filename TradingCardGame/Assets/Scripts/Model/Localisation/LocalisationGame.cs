using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LocalisationGame
{
    private static LocalisationGame instance;
    public static LocalisationGame Instance {
        get {
            if (instance == null)
                return instance = new LocalisationGame();
            return instance;
        }
    }

    public Action LanguageChanged;
    public string CurrentLanguage { get; private set; }
    public List<string> Localisations { get; private set; } = new List<string>();
    private Dictionary<string, string> data = new Dictionary<string, string>();

    private readonly string path = $"{Application.dataPath}/StreamingAssets/Localisation/";

    private LocalisationGame()
    {
        // Получить список всех локализаций
        var arrayDir = (new DirectoryInfo(path)).GetFiles("*.json");

        if (arrayDir.Length == 0)
            throw new Exception("Localization files not found!");

        foreach (var item in arrayDir)
            Localisations.Add(item.Name.Replace(".json", ""));

        ChangeLanguage(PlayerPrefs.GetString("Language", "english"));
    }

    public string GetLocalisationString(string key)
    {
        if (data.TryGetValue(key, out string value))
            return value;
        return $"Noname key ({key})";
    }

    public void ChangeLanguage(string newLanguage)
    {
        if (CurrentLanguage == newLanguage) return; //Данная локализация уже выбрана

        LoadLocalisation(CurrentLanguage = newLanguage);
        LanguageChanged?.Invoke();
    }

    private void LoadLocalisation(string newLanguage)
    {
        var pathNewLanguageFile = $"{path}{newLanguage}.json";

        if (File.Exists(pathNewLanguageFile) == false)
            throw new Exception($"File does not exist! { path }{ newLanguage}.json");

        using (var sr = new StreamReader(pathNewLanguageFile))
        {
            data = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());

            // Если данные не будут правильно загружены, то будет создана пустая локализация, не содержащая строк
            if (data == null)
                data = new Dictionary<string, string>();
        }
    }
}