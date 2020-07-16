using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class CollectionScene : BaseScene, IInitializable
{
    private List<ICollectionPanelUI> panels;
    private ICollectionPanelUI currentPanel;
    private IFractionsData fractions;

    [SerializeField] private FractionsMenu fractionMenu = null;
    [SerializeField] private Transform mainMenu = null;
    [SerializeField] private Button backMainMenuButton = null;
    [SerializeField] private List<CollectionButton> menuButtons = null;
    private CollectionButton currentButton;

    [Inject]
    public void InjectMetod(IFractionsData fractions, List<ICollectionPanelUI> panels)
    {
        (this.fractions, this.panels) = (fractions, panels);
    }

    public void Initialize()
    {
        panels.ForEach(x => x.Build(transform));

        menuButtons.ForEach(x => x.AddListener(OnSelectPanel));
        mainMenu.SetAsLastSibling();

        backMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(ScenesEnum.MainScene.ToString()));
        backMainMenuButton.transform.SetAsLastSibling();

        fractionMenu.Initialize(fractions.Fractions);
        OnSelectPanel(CollectionPanelsEnum.Decks);
    }

    private void OnSelectPanel(CollectionPanelsEnum type)
    {
        currentPanel?.Disable();
        (currentPanel = panels.Where(x => x.TypePanel == type).FirstOrDefault())?.Enable(fractionMenu);

        if (currentButton != null) currentButton.SetActive(false);
        (currentButton = menuButtons.Where(x => x.TypePanal == type).FirstOrDefault()).SetActive(true);
    }
}