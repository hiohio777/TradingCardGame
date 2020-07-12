using System;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : FactoryBase<CardUI>, ICardFactory<ICard>
{
    private readonly ISpecificityFactory specificityFactory;

    public CardFactory(ISpecificityFactory specificityFactory) => 
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
        CardUI cardUI;

        if (buffer.Count > 0) cardUI = buffer.Pop();
        else
        {
            cardUI = UnityEngine.Object.Instantiate(Resources.Load<CardUI>($"Card/CardUI")).Initialize(Buffered, specificityFactory);
            cardUI.SetCardUIStatus(CardUIStatus.CreatPrefab(cardUI.transform));
        }

        cardUI.Build(cardData, scale);

        return cardUI;
    }
}
