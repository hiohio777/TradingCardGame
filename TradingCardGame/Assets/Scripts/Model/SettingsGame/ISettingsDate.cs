using System.Collections.Generic;

public interface ISettingsData
{
    string CurrentLanguage { get; }
    IEnumerable<string> Localisations { get; }
    void LanguageChanged(string CurrentLanguage);
}