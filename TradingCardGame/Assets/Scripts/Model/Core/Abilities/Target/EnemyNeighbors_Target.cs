using System.Collections.Generic;
using UnityEngine;

public class EnemyNeighbors_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        var cards = new List<IAttackCard>();
        if (card.Id + 1 < card.EnemyPerson.AttackCards.Count)
            cards.Add(card.EnemyPerson.AttackCards[card.Id + 1]);
        if (card.Id - 1 >= 0)
            cards.Add(card.EnemyPerson.AttackCards[card.Id - 1]);
        return cards;
    }
}
