using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattelPersonPlayer : BattelPersonBase, IBattelPerson
{
    public Vector3 startPositionReservCard = new Vector3(0, -1000, 0);

    public BattelPersonPlayer(IFractionsData fractions, ICollectionCardsData collection, ICardFactory<IBattelCard> cardFactory)
    : base(fractions, collection, cardFactory)
    { }

    public override void NewStartingHand()
    {
        //Вернуть карты из руки в колоду
        ReservCards.ForEach(x => { DeckCards.Add(x.Combat.CardData); x.Destroy(); });
        ReservCards.Clear();

        ShuffleCards(DeckCards); //Перетасовать
        AddCardsReserve(startPositionReservCard);

        new StartingHandLocation(ReservCards);
    }

    public override void MoveToReservLocation(Action actEndRelocation, float yPosition = -450, int offset = 140)
    {
        new ReservLocation(ReservCards, actEndRelocation, yPosition, offset);
    }

    public void BringCardsToBattlefield(float animationTime, Action actEndRelocation)
    {
        var cardAttackList = new List<IAttackCard>();
        for (int i = 0; i < AttackCards.Count; i++)
        {
            if (AttackCards[i].BattelCard == null)
            {
                var card = ReservCards[0];
                AttackCards[i].PutCardFromReserve(card);
                ReservCards.Remove(card);
                cardAttackList.Add(AttackCards[i]);
            }
        }

        Action actFinish = () =>
        {
            cardAttackList.ForEach(x => { x.BattelCard.SetScale(new Vector3(1, 1, 1)); x.BattelCard.SetSortingOrder(0); });
            AddCardsReserve(startPositionReservCard);
            MoveToReservLocation(actEndRelocation);
        };

        if (cardAttackList.Count > 0)
        {
            new CardsRelocation(cardAttackList, actFinish, 0.5f);
        }
        else actFinish?.Invoke();
    }
}