using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;


public interface IBattelPersonEnemy : IBattelPerson { }
public class BattelPersonEnemy : BattelPersonBase, IBattelPersonEnemy
{
    private Vector3 startPositionReserv = new Vector3(0, +1000, 0);

    public BattelPersonEnemy(IFractionsData fractions, ICollectionCardsData collection, ICardFactory<IAttackCard> cardFactory)
    : base(fractions, collection, cardFactory)
    { TypePerson = TypePersonEnum.enemy; }

    public override void BringCardsToBattlefield(float animationTime, Action actEndRelocation)
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
                cardAttackList.ForEach(x => { x.View.SetScale(new Vector3(1, 1, 1)).SetSortingOrder(0); });

                for (int i = 0; i < data.cardReserv.Count; i++)
                    if (i >= ReservCards.Count)
                    {
                        CreatCard(collection.GetCard(data.cardReserv[i]), startPositionReserv);
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
            CreatCard(collection.GetCard(item), startPositionReserv);
        }
    }
}