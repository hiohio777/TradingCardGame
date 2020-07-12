using System.Collections.Generic;
using UnityEngine;

public class FriendlyAll_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        var cards = new List<IAttackCard>(card.FriendPerson.AttackCards);
        cards.Remove(card);
        return cards;
    }
}
