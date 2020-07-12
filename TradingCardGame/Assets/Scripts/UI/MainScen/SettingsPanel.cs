using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : PanelUI, IPanelUI
{
    private ISettingsData settingsData;

    [SerializeField] private Text languageText = null;
    [SerializeField] private Transform languagePanel = null;

    public IPanelUI Initialize(ISettingsData settingsData)
    {
        this.settingsData = settingsData;

        foreach (var nameLocalization in settingsData.Localisations)
        {
            var button = Instantiate(Resources.Load<LanguageButton>($"MainScene/LanguageButton"), languagePanel, false)
                .Initialize(nameLocalization, SelectNewLanguage);
            if (nameLocalization == settingsData.CurrentLanguage) button.Enable();
        }

        return this;
    }

    private void SelectNewLanguage(string newLanguage)
    {
        settingsData.LanguageChanged(newLanguage);
        languageText.text = $"{LocalisationGame.Instance.GetLocalisationString("language")}: {LocalisationGame.Instance.GetLocalisationString(newLanguage)}";
    }

}
