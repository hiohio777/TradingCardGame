using System;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{
    private Action<CollectionPanelsEnum> clicKButton;

    public CollectionPanelsEnum TypePanal => typePanal;
    [SerializeField] private CollectionPanelsEnum typePanal = CollectionPanelsEnum.Decks;

    private Button button;

    public void AddListener(Action<CollectionPanelsEnum> action) => clicKButton = action;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        if (active) button.image.color = Color.green;
        else button.image.color = Color.gray;
    }

    private void OnClickButton() => clicKButton.Invoke(typePanal);
}

