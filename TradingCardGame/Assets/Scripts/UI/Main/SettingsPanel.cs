using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsPanel : PanelUI, IPanelUI
{
    private ISettingsData settingsData;

    [SerializeField] private Text languageText;
    [SerializeField] private Transform languagePanel;

    [Inject]
    public void InjectMetod(ISettingsData settingsData)
    {
        this.settingsData = settingsData;
    }

    protected override void Initialize()
    {
        foreach (var nameLocalization in settingsData.Localisations)
        {
            var button = Instantiate(Resources.Load<LanguageButton>($"UI/ElementsUI/LanguageButton"), languagePanel, false)
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
