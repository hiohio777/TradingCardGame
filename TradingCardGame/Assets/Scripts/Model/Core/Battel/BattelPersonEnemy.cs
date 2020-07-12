using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattelPersonEnemy : BattelPersonBase, IBattelPerson
{
    private Vector3 startPositionReservCard = new Vector3(0, +1000, 0);

    public BattelPersonEnemy(IFractionsData fractions, ICollectionCardsData collection, ICardFactory<IBattelCard> cardFactory)
    : base(fractions, collection, cardFactory)
    { }

    public override void MoveToReservLocation(Action action, float yPosition = -450, int offset = 140) 
    {
        throw new NotImplementedException("MoveToReservLocation(): Недопустимо использовать этот медод для врага");
    }

    public void BringCardsToBattlefield(float animationTime, Action actEndRelocation)
    {
        var data = JsonConvert.DeserializeObject<PersonDATAREPORT>(Report);

        var cardAttackList = new List<IAttackCard>();
        for (int i = 0; i < AttackCards.Count; i++)
            if (AttackCards[i].BattelCard == null)
            {
                var card = ReservCards.Where(x => x.Combat.Name == data.cardsAttack[i]).FirstOrDefault();
                AttackCards[i].PutCardFromReserve(card);
                ReservCards.Remove(card);
                cardAttackList.Add(AttackCards[i]);
            }

        if (cardAttackList.Count > 0)
        {
            Action actFinish = () =>
            {
                cardAttackList.ForEach(x => { x.BattelCard.SetScale(new Vector3(1, 1, 1)); x.BattelCard.SetSortingOrder(0); });

                for (int i = 0; i < data.cardReserv.Count; i++)
                    if (i >= ReservCards.Count)
                    {
                        var dataCard = collection.GetCard(data.cardReserv[i]);
                        ReservCards.Add(cardFactory.GetCard(dataCard, new Vector3(1.3f, 1.3f, 1.3f)).SetPosition(startPositionReservCard) as IBattelCard);
                        DeckCards.RemoveAt(0);
                    }

                actEndRelocation?.Invoke();
            };

            new CardsRelocation(cardAttackList, actFinish, 0.5f);
        }
        else actEndRelocation?.Invoke();
    }

    public override void NewStartingHand()
    {
        var dataTemp = JsonConvert.DeserializeObject<List<string>>(Report);
        foreach (var item in dataTemp)
        {
            var dataCard = collection.GetCard(item);
            ReservCards.Add(cardFactory.GetCard(dataCard, new Vector3(1.3f, 1.3f, 1.3f)).SetPosition(startPositionReservCard) as IBattelCard);
            DeckCards.Remove(dataCard);
        }
    }
}