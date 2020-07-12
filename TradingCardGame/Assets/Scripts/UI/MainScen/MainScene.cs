using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : BaseScene
{
    private List<IPanelUI> panels;
    private IPanelUI current;
    private static MainMenuPanelsEnum currentEnum = MainMenuPanelsEnum.def;

    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private Button backMainMenuButton = null, quitGameButton = null, collectionSceneButton = null;
    [SerializeField] private List<MainMenuButton> menuButtons = null;

    public MainScene Initialize(Action<ScenesEnum> startNewScen, Func<List<IPanelUI>> creatorPanels)
    {
        (this.startNewScene, panels) = (startNewScen, creatorPanels.Invoke());
        panels.ForEach(x => x.SetParent(transform));
        menuButtons.ForEach(x => x.SetListener(OnSelectButton));
        quitGameButton.onClick.AddListener(QuitGame);
        collectionSceneButton.onClick.AddListener(StartCollectionScene);

        backMainMenuButton.onClick.AddListener(() => SetActiveMainMenu(true));
        backMainMenuButton.transform.SetParent(null, false);

        if (currentEnum != MainMenuPanelsEnum.def)
            OnSelectButton(currentEnum);

        return this;
    }

    private void SetActiveMainMenu(bool active)
    {
        current?.Disable();
        mainMenu.SetActive(active);
        currentEnum = MainMenuPanelsEnum.def;
    }

    private void OnSelectButton(MainMenuPanelsEnum button)
    {
        SetActiveMainMenu(false);
        (current = panels.Where(x => x.TypePanel == button).FirstOrDefault())?.Enable();
        backMainMenuButton.transform.SetParent(current.TransformPanel, false);
        currentEnum = button;
    }

    private void StartCollectionScene()
    {
        StartNewScen(ScenesEnum.CollectionScene);
    }
    private void QuitGame()
    {
        Debug.Log("GameExit!");
        Application.Quit();
    }
}
