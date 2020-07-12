using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsCollectionPanel : BaseCollectionPanelUI, ICollectionPanelUI
{
    private IFractionsData fractions;

    [SerializeField] private CardsPanelUI cardsPanel = null;

    public ICollectionPanelUI Initialize(ICardFactory<ICard> cardFactory, ICollectionCardsData collection, IFractionsData fractions)
    {
        this.fractions = fractions;
        cardsPanel.Build(cardFactory, collection, OnClickCard, fractions);

        return this;
    }

    public override void Enable(FractionsMenu fractionMenu)
    {
        base.Enable(fractionMenu);
        fractionMenu.SetActiveBattons(fractions.Fractions).SetListener(SelectFraction);
        fractionMenu.SetSelecedButton(fractions.CurrentFraction);
    }

    public override void Disable()
    {
        cardsPanel.DestroyCardUI();
        base.Disable();
    }

    private void SelectFraction(object fraction) =>
         cardsPanel.SelectFraction(fraction as IFraction);

    private void OnClickCard(ICard card) { }
}