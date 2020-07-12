using System;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPanel : PanelUI, IPanelUI
{
    private Action<ScenesEnum> startNewScene;
    private IUserDecks decksCollection;

    [SerializeField] private List<BattelMenuButton> menuButtons = null;

    public IPanelUI Initialize(Action<ScenesEnum> startNewScene, IUserDecks decksCollection)
    {
        (this.startNewScene, this.decksCollection) = (startNewScene, decksCollection);
        menuButtons.ForEach(x => x.SetListener(OnSelectBattel));

        return this;
    }

    private void OnSelectBattel(TypeBattelEnum typeBattel)
    {
        if (decksCollection.Decks.Count == 0)
        {
            MessagePanel.SimpleMessage(transform, "no_decks");
            return;
        }

        switch (typeBattel)
        {
            case TypeBattelEnum.training: startNewScene.Invoke(ScenesEnum.TrainingBattelScenes); break;
            case TypeBattelEnum.common: startNewScene.Invoke(ScenesEnum.CommonBattelScenes); break;
            case TypeBattelEnum.rating: startNewScene.Invoke(ScenesEnum.RatingBattelScenes); break;
        }
    }
}
