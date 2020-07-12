using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Общие настройки игры
/// </summary>
public class SettingsData : ISettingsData
{
    public string CurrentLanguage => LocalisationGame.Instance.CurrentLanguage;
    public List<string> Localisations => LocalisationGame.Instance.Localisations;

    public ISettingsData Load()
    {
        LocalisationGame.Instance.ChangeLanguage(PlayerPrefs.GetString("Language", "english"));
        return this;
    }

    public void LanguageChanged(string CurrentLanguage)
    {
        LocalisationGame.Instance.ChangeLanguage(CurrentLanguage);
        PlayerPrefs.SetString("Language", CurrentLanguage);
    }
}