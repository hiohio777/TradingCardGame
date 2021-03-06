﻿using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattelPersonBase : IBattelPerson
{
    public event Action<int> SetLive;
    public event Action<bool> SetFortune;
    public string Name { get; private set; }
    public IFraction Fraction { get; private set; }
    public bool IsReadyContinue { get; set; }
    public TypePersonEnum TypePerson { get; protected set; }
    public List<IAttackCard> ReservCards { get; private set; } = new List<IAttackCard>();
    public List<IAttackCard> AttackCards { get; private set; } = new List<IAttackCard>();
    public List<ICellBattel> Cell { get; set; }
    public IBattelPerson EnemyPerson { get; set; }
    public bool Fortune { get => fortune; set { fortune = value; SetFortune?.Invoke(fortune); } }
    public int Live { get => live; set { live = value; SetLive?.Invoke(live); } }
    public string Report { get; set; }

    public List<ICardData> DeckCards { get; set; }
    private int live;
    private bool fortune;

    protected static byte сountCardsHand = 6; //Количество карт в руке
    protected readonly IFractionsData fractions;
    protected readonly ICollectionCardsData collection;
    protected readonly ICardFactory<IAttackCard> cardFactory;

    public BattelPersonBase(IFractionsData fractions, ICollectionCardsData collection, ICardFactory<IAttackCard> cardFactory) =>
        (this.fractions, this.collection, this.cardFactory) = (fractions, collection, cardFactory);

    public void AssingCells(List<ICellBattel> cell) => Cell = cell;

    public void Creat(string name, string fraction, List<string> cards, byte live = 10)
    {
        (Name, this.live) = (name, live);

        Fraction = fractions.GetFraction(fraction);
        DeckCards = collection.GetCards(cards);
        ShuffleCards(DeckCards);
    }

    protected void AddCardsReserve(Vector3 startPosition)
    {
        int count = сountCardsHand - ReservCards.Count;
        for (int i = 0; i < count; i++)
        {
            CreatCard(DeckCards[0], startPosition);
        }
    }

    public void ShuffleCards(List<ICardData> cards)
    {
        var RND = new System.Random();
        for (int i = 0; i < cards.Count; i++)
        {
            var tmp = cards[0];
            cards.RemoveAt(0);
            cards.Insert(RND.Next(cards.Count), tmp);
        }
    }

    public abstract void NewStartingHand();

    public void PlaceAttackCell(IAttackCard card, ICellBattel cell, bool isMoving = true, Action finish = null)
    {
        AttackCards.Add(card);
        ReservCards.Remove(card);
        card.PlaceAttackCell(cell, isMoving, finish);
    }

    protected void CreatCard(ICardData cardData, Vector3 position)
    {
        var card = cardFactory.GetCard(cardData, new Vector3(1.3f, 1.3f, 1.3f));
        card.View.SetPosition(position);

        card.Warrior.FriendPerson = this;
        card.Warrior.EnemyPerson = EnemyPerson;

        ReservCards.Add(card);
        DeckCards.RemoveAt(0);
    }

    public abstract void BringCardsToBattlefield(float animationTime, Action actEndRelocation);
}
