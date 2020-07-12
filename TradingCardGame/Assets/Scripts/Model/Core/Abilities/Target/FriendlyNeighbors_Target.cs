using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendlyNeighbors_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        var cards = new List<IAttackCard>();
        if (card.Id + 1 < card.FriendPerson.AttackCards.Count)
            cards.Add(card.FriendPerson.AttackCards[card.Id + 1]);
        if (card.Id - 1 >= 0)
            cards.Add(card.FriendPerson.AttackCards[card.Id - 1]);

        return cards;
    }
}