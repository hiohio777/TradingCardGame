using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsPanel : PanelUI, IPanelUI
{
    public PanelNameEnum Name { get; } = PanelNameEnum.Settings;
    public bool IsActiveReturnButton => true;
    private ISettingsData settingsData;

    [SerializeField] private Text languageText = null;
    [SerializeField] private Transform languagePanel = null;
    private bool isCreatPanal = false;

    [Inject]
    public void InjectMetod(ISettingsData settingsData, BaseGameButton<bool> returnButton)
    {
        this.settingsData = settingsData;
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
        if (isCreatPanal) return;
        isCreatPanal = true;
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
