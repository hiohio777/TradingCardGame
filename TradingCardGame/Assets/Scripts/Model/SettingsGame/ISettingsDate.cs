using System.Collections.Generic;

public interface ISettingsData
{
    string CurrentLanguage { get; }
    List<string> Localisations { get; }
    void LanguageChanged(string CurrentLanguage);
    ISettingsData Load();
}