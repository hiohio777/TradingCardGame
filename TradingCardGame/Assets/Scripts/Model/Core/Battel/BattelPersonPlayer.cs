using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattelPersonPlayer : BattelPersonBase, IBattelPerson
{
    public Vector3 startPositionReservCard = new Vector3(0, -1000, 0);

    public BattelPersonPlayer(IFractionsData fractions, ICollectionCardsData collection, ICardFactory<IAttackCard> cardFactory)
    : base(fractions, collection, cardFactory)
    { TypePerson = TypePersonEnum.player; }

    public override void NewStartingHand()
    {
        //Вернуть карты из руки в колоду
        ReservCards.ForEach(x => { DeckCards.Add(x.Combat.CardData); x.Destroy(); });
        ReservCards.Clear();

        ShuffleCards(DeckCards); //Перетасовать
        AddCardsReserve(startPositionReservCard);

        new StartingHandLocation(ReservCards);
    }

    public void BringCardsToBattlefield(float animationTime, Action actEndRelocation)
    {
        var cardAttackList = new List<IAttackCard>();
        for (int i = 0; i < Cell.Count; i++)
        {
            if (Cell[i].IsExist == false)
            {
                var card = ReservCards[0];
                PlaceAttackCell(card, Cell[i]);
                cardAttackList.Add(AttackCards[i]);
            }
        }

        Action actFinish = () =>
        {
            cardAttackList.ForEach(x => { x.SetScale(new Vector3(1, 1, 1)); x.SetSortingOrder(0); });
            AddCardsReserve(startPositionReservCard);
            ReservCards.ReservLocation(actEndRelocation);
        };

        if (cardAttackList.Count > 0)
        {
            cardAttackList.Relocation(actFinish, 0.5f);
        }
        else actFinish?.Invoke();
    }
}