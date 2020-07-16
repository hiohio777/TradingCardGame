using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuPanel : PanelUI, IPanelUI
{
    public PanelNameEnum Name { get; } = PanelNameEnum.MainMenu;

    [SerializeField] private Button quitGame;
    [SerializeField] private List<MainMenuButton> menuButtons;
    private BaseGameButton<bool> returnButton;

    [Inject]
    public void InjectMetod(BaseGameButton<bool> returnButton)
    {
        this.returnButton = returnButton;
        menuButtons.ForEach(x => x.SetListener(OnOpenSubPanel));
        quitGame.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Debug.Log("GameExit!");
        Application.Quit();
    }

    public override void Enable()
    {
        returnButton.SetActive(false);
        base.Enable();
    }
}