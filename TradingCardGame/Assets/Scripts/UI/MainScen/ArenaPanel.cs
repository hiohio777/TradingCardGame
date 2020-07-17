using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ArenaPanel : PanelUI, IPanelUI
{
    private IUserData user;
    [SerializeField] private List<BattelMenuButton> menuButtons = null;

    [Inject]
    public void InjectMetod(IUserData user)
    {
        this.user = user;
        menuButtons.ForEach(x => x.SetListener(OnSelectBattel));
    }

    protected override void Initialize() { }

    private void OnSelectBattel(object sender, TypeBattelEnum typeBattel)
    {
        if (user.Decks.Count == 0)
        {
            MessagePanel.SimpleMessage(transform, "no_decks");
            return;
        }
        SceneManager.LoadScene(typeBattel.ToString());
    }
}
