using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionCardsData : ICollectionCardsData
{
    public Dictionary<string, ICardData> Cards { get; private set; }
    private readonly List<ICardData> cardsInitiative;

    public CollectionCardsData(IFractionsData fractions)
    {
        Cards = LoadCollectionCard(fractions);
        (cardsInitiative = Cards.Values.ToList()).Sort(CompareTo);
    }
    public List<ICardData> GetFractionCards(string fraction, bool isOnlyOpenCards = false)
    {
        var listCards = new List<ICardData>();

        if (isOnlyOpenCards == false) //Все карты
            return cardsInitiative.Where(x => x.Fraction.Name == fraction).ToList();
        //Только открытые карты
        return cardsInitiative.Where(x => x.IsOpen == isOnlyOpenCards && x.Fraction.Name == fraction).ToList();
    }
    public List<ICardData> GetCards(List<string> names)
    {
        var temp = new List<ICardData>();
        foreach (var item in names)
        {
            var card = GetCard(item);
            if (card == null) throw new Exception($"Запрашиваемой карты не существует! {item}");
            temp.Add(card);
        }

        temp.Sort(CompareTo);

        return temp;
    }
    public ICardData GetCard(string name) => Cards[name];
    public void OpenCard(string name) => Cards[name].Open();
    private int CompareTo(ICardData x, ICardData y)
    {
        if (x.Initiative < y.Initiative)
            return -1;
        return 1;
    }
    private Dictionary<string, ICardData> LoadCollectionCard(IFractionsData fractions)
    {
        var cards = new Dictionary<string, ICardData>();
        foreach (var fraction in fractions.Fractions)
            foreach (var item in Resources.LoadAll<CardScriptable>($"Data/Cards/{fraction.Name}/"))
                cards.Add(item.Name, item.Open());
        return cards;
    }
}