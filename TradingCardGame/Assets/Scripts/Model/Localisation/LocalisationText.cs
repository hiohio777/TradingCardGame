using UnityEngine;
using UnityEngine.UI;

public class LocalisationText : MonoBehaviour
{
    [SerializeField] private string Key;
    private Text TextComponent;

    public Color Color { set => TextComponent.color = value; }

    public void SetKey(string newKey)
    {
        Key = newKey;
        SetLocalisationString();
    }

    private void Awake()
    {
        TextComponent = GetComponent<Text>();
    }

    private void Start()
    {
        LocalisationGame.Instance.LanguageChanged += SetLocalisationString;
        SetLocalisationString();
    }

    private void OnDestroy() => LocalisationGame.Instance.LanguageChanged -= SetLocalisationString;
    private void SetLocalisationString() => TextComponent.text = LocalisationGame.Instance.GetLocalisationString(Key);
}