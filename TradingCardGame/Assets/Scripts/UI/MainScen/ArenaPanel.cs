using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ArenaPanel : PanelUI, IPanelUI, IInitializable
{
    private IUserData decksCollection;

    [SerializeField] private List<BattelMenuButton> menuButtons = null;

    [Inject]
    public void InjectMetod(IUserData decksCollection)
    {
        this.decksCollection = decksCollection;
    }

    public void Initialize()
    {
        menuButtons.ForEach(x => x.SetListener(OnSelectBattel));
    }

    private void OnSelectBattel(TypeBattelEnum typeBattel)
    {
        if (decksCollection.Decks.Count == 0)
        {
            MessagePanel.SimpleMessage(transform, "no_decks");
            return;
        }
        SceneManager.LoadScene(typeBattel.ToString());
    }
}
