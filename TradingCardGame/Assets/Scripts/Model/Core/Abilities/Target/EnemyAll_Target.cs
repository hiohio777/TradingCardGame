using System.Collections.Generic;
using UnityEngine;

public class EnemyAll_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel) =>
         new List<IAttackCard>(card.Warrior.EnemyPerson.AttackCards);
}
