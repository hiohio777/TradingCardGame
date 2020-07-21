using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IBattelPersonPlayer : IBattelPerson { }
public class BattelPersonPlayer : BattelPersonBase, IBattelPersonPlayer
{
    public Vector3 startPositionReservCard = new Vector3(0, -1000, 0);

    public BattelPersonPlayer(IFractionsData fractions, ICollectionCardsData collection, ICardFactory<IAttackCard> cardFactory)
    : base(fractions, collection, cardFactory)
    { TypePerson = TypePersonEnum.player; }

    public override void NewStartingHand()
    {
        //Вернуть карты из руки в колоду
        ReservCards.ForEach(x => { DeckCards.Add(x.Combat.CardData); x.DestroyUI(); });
        ReservCards.Clear();

        ShuffleCards(DeckCards); //Перетасовать
        AddCardsReserve(startPositionReservCard);

        ReservCards.StartingHandLocation();
    }

    public override void BringCardsToBattlefield(float animationTime, Action actEndRelocation)
    {
        var cardAttackList = new List<IAttackCard>();
        for (int i = 0; i < Cell.Count; i++)
        {
            if (Cell[i].IsExist == false)
            {
                var card = ReservCards[0];
                PlaceAttackCell(card, Cell[i], false);
                cardAttackList.Add(AttackCards.Last());
            }
        }

        Action actFinish = () =>
        {
            cardAttackList.ForEach(x => { x.View.SetScale(new Vector3(1, 1, 1)).SetSortingOrder(0); });
            AddCardsReserve(startPositionReservCard);
            ReservCards.ReservLocation(actEndRelocation);
        };

        if (cardAttackList.Count > 0)
        {
            cardAttackList.Relocation(actFinish, 0.3f);
        }
        else actFinish?.Invoke();
    }
}
