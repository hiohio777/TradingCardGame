using System;

public class CardsPanelEditDeckUI : BaseCardsPanal
{
    private IFractionsData fractions;
    private IDeckData deck;

    public void Build(ICardFactory<ICard> cardFactory, ICollectionCardsData collection, Action<ICard> clickCard, IFractionsData fractions)
    {
        this.fractions = fractions;
        base.Build(cardFactory, collection, clickCard);
    }

    public void SetDeck(IDeckData deck) => this.deck = deck;

    public void SelectFraction(IFraction fraction)
    {
        fractions.CurrentFraction = fraction;
        cards = collection.GetFractionCards(fractions.CurrentFraction.Name, true);

        FirstPages(cards.Count);
    }

    public void UpdatePanel()
    {
        cards = collection.GetFractionCards(fractions.CurrentFraction.Name, true);
        CarrentPages(cards.Count);
    }

    protected override void CreatCardUI(int i)
    {
        var card = cardFactory.GetCard(cards[i]).SetParent(transform).SetSortingOrder(i);
        card.SetClickListener(OnClickCard);
        cardsUI.Add(card);

        // Определить статус карты относительно колоды
        if (deck.Cards.Contains(card.CardData))
        {
            card.Status = StatusCardEnum.in_the_deck;
            return;
        }

        if(deck.IsInitiativeCards(card.CardData))
        {
            card.Status = StatusCardEnum.not_available;
            return;
        }

        card.Status = StatusCardEnum.normal;
    }
}