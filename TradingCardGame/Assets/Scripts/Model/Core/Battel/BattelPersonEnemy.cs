using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattelPersonEnemy : BattelPersonBase, IBattelPerson
{
    private Vector3 startPositionReservCard = new Vector3(0, +1000, 0);

    public BattelPersonEnemy(IFractionsData fractions, ICollectionCardsData collection, ICardFactory<IAttackCard> cardFactory)
    : base(fractions, collection, cardFactory)
    { TypePerson = TypePersonEnum.enemy; }

    public void BringCardsToBattlefield(float animationTime, Action actEndRelocation)
    {
        var data = JsonConvert.DeserializeObject<PersonDATAREPORT>(Report);

        var cardAttackList = new List<IAttackCard>();
        for (int i = 0; i < Cell.Count; i++)
            if (Cell[i].IsExist == false)
            {
                var card = ReservCards.Where(x => x.Combat.Name == data.cardsAttack[i]).FirstOrDefault();
                PlaceAttackCell(card, Cell[i]);
                cardAttackList.Add(card);
            }

        if (cardAttackList.Count > 0)
        {
            Action actFinish = () =>
            {
                cardAttackList.ForEach(x => { x.SetScale(new Vector3(1, 1, 1)); x.SetSortingOrder(0); });

                for (int i = 0; i < data.cardReserv.Count; i++)
                    if (i >= ReservCards.Count)
                    {
                        var dataCard = collection.GetCard(data.cardReserv[i]);
                        ReservCards.Add(cardFactory.GetCard(dataCard, new Vector3(1.3f, 1.3f, 1.3f)).SetPosition(startPositionReservCard) as IAttackCard);
                        DeckCards.RemoveAt(0);
                    }

                actEndRelocation?.Invoke();
            };

            cardAttackList.Relocation(actFinish, 0.5f);
        }
        else actEndRelocation?.Invoke();
    }



    public override void NewStartingHand()
    {
        var dataTemp = JsonConvert.DeserializeObject<List<string>>(Report);
        foreach (var item in dataTemp)
        {
            var dataCard = collection.GetCard(item);
            ReservCards.Add(cardFactory.GetCard(dataCard, new Vector3(1.3f, 1.3f, 1.3f)).SetPosition(startPositionReservCard) as IAttackCard);
            DeckCards.Remove(dataCard);
        }
    }

    public List<ICellBattel> Cell { get; set; }
}