using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckData : IDeckData
{
    public string Name { get; set; }
    public string Fraction { get; set; }
    public StatusDeckEnum Status { get; set; } = StatusDeckEnum.Available;
    public List<string> StringCards { get; set; } = new List<string>();
    public List<ICardData> Cards { get; set; } = new List<ICardData>();

    private const byte max = 6, maxCountAllCards = 24; //Максимальное количество карт, по инициативе и вцелом
    private byte[] initiativeCards = new byte[4];

    private DeckData() {  }
    public DeckData(string name, string fraction, StatusDeckEnum status) =>
        (Name, Fraction, Status) = (name, fraction, status);
    public DeckData(string name, string fraction, List<string> stringCards, List<ICardData> cards)
    {
        (Name, Fraction, StringCards, Cards) = (name, fraction, stringCards, cards);
        Verify();
    }

    public bool IsInitiativeCards(ICardData card)
    {
        if (initiativeCards[(int)card.TypeInitiative] >= max) return true; //Мест нет для карты с этим TypeInitiative
        return false;
    }

    public bool AddCard(ICardData cardData)
    {
        if (Cards.Contains(cardData) || initiativeCards[(int)cardData.TypeInitiative] >= max)
            return false;

        initiativeCards[(int)cardData.TypeInitiative]++;
        StringCards.Add(cardData.Name);
        Cards.Add(cardData);

        return true;
    }

    public bool RemoveCard(ICardData cardData)
    {
        initiativeCards[(int)cardData.TypeInitiative]--;
        StringCards.Remove(cardData.Name);
        Cards.Remove(cardData);

        return true;
    }

    public IDeckData Clone()
    {
        return new DeckData()
        {
            Name = Name,
            Fraction = Fraction,
            Status = Status,
            StringCards = new List<string>(StringCards),
            Cards = new List<ICardData>(Cards),
            initiativeCards = initiativeCards,
        };
    }

    public void PutClon(IDeckData clon)
    {
        (Name, Fraction, StringCards, Cards) = (clon.Name, clon.Fraction, clon.StringCards, clon.Cards);
        Verify();
    }

    private void Verify()
    {
        initiativeCards = new byte[4];
        Cards.ForEach(x => initiativeCards[(int)x.TypeInitiative]++);

        for (int i = 0; i < initiativeCards.Length; i++)
            if (initiativeCards[i] > max)
            {
                Status = StatusDeckEnum.Broken;
                return;
            }

        if (Cards.Count < maxCountAllCards)
        {
            Status = StatusDeckEnum.Broken;
            return;
        }

        Status = StatusDeckEnum.Available;
    }
}