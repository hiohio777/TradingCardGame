using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacked_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        if (card.Warrior.AttackTargetID != -1)
            return new List<IAttackCard>() { card.Warrior.AttackTargetUnit };

        return new List<IAttackCard>();
    }
}
