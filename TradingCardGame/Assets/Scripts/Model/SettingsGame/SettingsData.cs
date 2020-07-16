using System.Collections.Generic;
using UnityEngine;

public class SettingsData : ISettingsData
{
    public SettingsData()
    {
    }

    public string CurrentLanguage => LocalisationGame.Instance.CurrentLanguage;
    public IEnumerable<string> Localisations => LocalisationGame.Instance.Localisations;

    public void LanguageChanged(string CurrentLanguage)
    {
        LocalisationGame.Instance.ChangeLanguage(CurrentLanguage);
        PlayerPrefs.SetString("Language", CurrentLanguage);
    }
}