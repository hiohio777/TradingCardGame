using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LanguageButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private LocalisationText localisationText;
    private Action<string> clicK;
    private string nameLanguage;
    private static LanguageButton current;

    public LanguageButton Initialize(string nameLanguage, Action<string> clicK)
    {
        localisationText.SetKey(this.nameLanguage = nameLanguage);
        Disable();
        this.clicK = clicK;

        return this;
    }

    public void Disable() => localisationText.text.color = Color.gray;
    public void Enable()
    {
        current = this;
        localisationText.text.color = Color.green;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (current == this) return;

        if (current != null) current.Disable();
        Enable();
        clicK?.Invoke(nameLanguage);
    }
}