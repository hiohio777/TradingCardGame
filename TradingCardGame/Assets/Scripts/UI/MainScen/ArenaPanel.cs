using System;
using System.Collections.Generic;
using UnityEngine;
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

        //switch (typeBattel)
        //{
        //    case TypeBattelEnum.training: startNewScene.Invoke(ScenesEnum.TrainingBattelScenes); break;
        //    case TypeBattelEnum.common: startNewScene.Invoke(ScenesEnum.CommonBattelScenes); break;
        //    case TypeBattelEnum.rating: startNewScene.Invoke(ScenesEnum.RatingBattelScenes); break;
        //}
    }
}
