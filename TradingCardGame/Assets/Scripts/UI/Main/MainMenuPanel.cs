using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuPanel : PanelUI, IPanelUI
{
    [SerializeField] private Button quitGame;
    [SerializeField] private List<MainMenuButton> menuButtons;
    private BaseGameButton returnButton;

    [Inject]
    public void InjectMetod(ReturnButton returnButton)
    {
        this.returnButton = returnButton;
        menuButtons.ForEach(x => x.SetListener(OnOpenSubPanel));
        quitGame.onClick.AddListener(QuitGame);
    }

    protected override void Initialize() { }

    private void QuitGame()
    {
        Debug.Log("GameExit!");
        Application.Quit();
    }

    public override void Enable()
    {
        base.Enable();
        returnButton.SetActive(false);
    }
}