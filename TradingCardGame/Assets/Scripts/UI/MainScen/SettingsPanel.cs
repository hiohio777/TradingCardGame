using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsPanel : PanelUI, IPanelUI, IInitializable
{
    private ISettingsData settingsData;

    [SerializeField] private Text languageText = null;
    [SerializeField] private Transform languagePanel = null;

    [Inject]
    public void InjectMetod(ISettingsData settingsData)
    {
        this.settingsData = settingsData;
    }

    public void Initialize()
    {
        foreach (var nameLocalization in settingsData.Localisations)
        {
            var button = Instantiate(Resources.Load<LanguageButton>($"MainScene/LanguageButton"), languagePanel, false)
                .Initialize(nameLocalization, SelectNewLanguage);
            if (nameLocalization == settingsData.CurrentLanguage) button.Enable();
        }
    }

    private void SelectNewLanguage(string newLanguage)
    {
        settingsData.LanguageChanged(newLanguage);
        languageText.text = $"{LocalisationGame.Instance.GetLocalisationString("language")}: {LocalisationGame.Instance.GetLocalisationString(newLanguage)}";
    }

}
