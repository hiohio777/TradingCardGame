using System.Collections.Generic;
using UnityEngine;

public class CardFactory : FactoryBase<Card>, ICardFactory<ICard>
{
    private readonly ISFXFactory specificityFactory;

    public CardFactory(ISFXFactory specificityFactory) =>
        this.specificityFactory = specificityFactory;

    public List<ICard> GetCards(List<ICardData> cardsData) => GetCards(cardsData, new Vector3(1, 1, 1));
    public ICard GetCard(ICardData cardData) => GetCard(cardData, new Vector3(1, 1, 1));

    public List<ICard> GetCards(List<ICardData> cardsData, Vector3 scale)
    {
        var cards = new List<ICard>();
        foreach (var item in cardsData)
            cards.Add(GetCard(item, scale));

        return cards;
    }

    public ICard GetCard(ICardData cardData, Vector3 scale)
    {
        Card card;

        if (buffer.Count > 0) card = buffer.Pop();
        else
        {
            card = UnityEngine.Object.Instantiate(Resources.Load<Card>($"Card/CardUI")).Initial(Buffered, specificityFactory);
            conteiner.Add(card);
        }

        card.Build(cardData, scale);
        return card;
    }
}
