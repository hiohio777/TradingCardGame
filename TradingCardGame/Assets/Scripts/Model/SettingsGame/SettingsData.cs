using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Общие настройки игры
/// </summary>
public class SettingsData : ISettingsData
{
    public SettingsData()
    {
        LocalisationGame.Instance.ChangeLanguage(PlayerPrefs.GetString("Language", "english"));
    }

    public string CurrentLanguage => LocalisationGame.Instance.CurrentLanguage;
    public IEnumerable<string> Localisations => LocalisationGame.Instance.Localisations;

    public void LanguageChanged(string CurrentLanguage)
    {
        LocalisationGame.Instance.ChangeLanguage(CurrentLanguage);
        PlayerPrefs.SetString("Language", CurrentLanguage);
    }
}