using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacked_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        if (card.AttackCardTarget != null) return new List<IAttackCard>() { card.AttackCardTarget };
        return new List<IAttackCard>();
    }
}
