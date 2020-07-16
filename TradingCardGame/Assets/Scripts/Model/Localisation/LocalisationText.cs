using UnityEngine;
using UnityEngine.UI;

public class LocalisationText : MonoBehaviour
{
    [SerializeField] private string Key;
    public Text text;

    public Color Color { set => text.color = value; }

    public void SetKey(string newKey)
    {
        Key = newKey;
        SetLocalisationString();
    }

    private void Start()
    {
        LocalisationGame.Instance.LanguageChanged += SetLocalisationString;
        SetLocalisationString();
    }

    private void OnDestroy() => LocalisationGame.Instance.LanguageChanged -= SetLocalisationString;
    private void SetLocalisationString() => text.text = LocalisationGame.Instance.GetLocalisationString(Key);
}