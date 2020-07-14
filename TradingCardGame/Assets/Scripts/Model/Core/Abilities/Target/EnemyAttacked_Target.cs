using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacked_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        if (card.AttackTarget != -1)
            return new List<IAttackCard>() { card.EnemyPerson.Cell[card.AttackTarget].Unit };

        return new List<IAttackCard>();
    }
}
