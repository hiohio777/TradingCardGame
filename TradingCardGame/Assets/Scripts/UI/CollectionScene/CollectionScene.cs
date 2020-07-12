using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionScene : BaseScene
{
    private List<ICollectionPanelUI> panels;
    private ICollectionPanelUI currentPanel;

    [SerializeField] private FractionsMenu fractionMenu = null;
    [SerializeField] private Transform mainMenu = null;
    [SerializeField] private Button backMainMenuButton = null;
    [SerializeField] private List<CollectionButton> menuButtons = null;
    private CollectionButton currentButton;

    public CollectionScene Initialize(Action<ScenesEnum> startNewScene, IFractionsData fractions, Func<List<ICollectionPanelUI>> creatorPanels)
    {
        (this.startNewScene, panels) = (startNewScene, creatorPanels.Invoke());
        panels.ForEach(x => x.Build(transform));

        menuButtons.ForEach(x => x.AddListener(OnSelectPanel));
        mainMenu.SetAsLastSibling();

        backMainMenuButton.onClick.AddListener(() => StartNewScen(ScenesEnum.MainScenes));
        backMainMenuButton.transform.SetAsLastSibling();

        fractionMenu.Initialize(fractions.Fractions);
        OnSelectPanel(CollectionPanelsEnum.Decks);

        return this;
    }

    private void OnSelectPanel(CollectionPanelsEnum type)
    {
        currentPanel?.Disable();
        (currentPanel = panels.Where(x => x.TypePanel == type).FirstOrDefault())?.Enable(fractionMenu);

        if(currentButton != null) currentButton.SetActive(false);
        (currentButton = menuButtons.Where(x => x.TypePanal == type).FirstOrDefault()).SetActive(true);
    }
}