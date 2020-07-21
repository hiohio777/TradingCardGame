using System;

public class CardsPanelUI : BaseCardsPanal
{
    private IFractionsData fractions;

    public void Build(ICardFactory<ICard> cardFactory, ICollectionCardsData collection, Action<ICard> clickCard, IFractionsData fractions)
    {
        this.fractions = fractions;
        base.Build(cardFactory, collection, clickCard);
    }

    public void SelectFraction(IFraction fraction)
    {
        fractions.CurrentFraction = fraction;
        cards = collection.GetFractionCards(fractions.CurrentFraction.Name, true);

        FirstPages(cards.Count);
    }
}
