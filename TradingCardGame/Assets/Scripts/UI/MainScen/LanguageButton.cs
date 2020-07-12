using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour, IPointerClickHandler
{
    private Action<string> clicK;
    private string nameLanguage;
    private Text nameLanguageText; 
    private static LanguageButton current;

    public LanguageButton Initialize(string nameLanguage, Action<string> clicK)
    {
        GetComponent<LocalisationText>().SetKey(this.nameLanguage = nameLanguage);
        nameLanguageText = GetComponent<Text>();
        Disable();
        this.clicK = clicK;

        return this;
    }

    public void Disable() => nameLanguageText.color = Color.gray;
    public void Enable()
    {
        current = this;
        nameLanguageText.color = Color.green;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (current == this) return;

        if (current != null) current.Disable();
        Enable();
        clicK?.Invoke(nameLanguage);
    }
}