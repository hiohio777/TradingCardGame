using System.Collections.Generic;
using UnityEngine;

public class FriendlyAllWithItself_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel) =>
         new List<IAttackCard>(card.Warrior.FriendPerson.AttackCards);
}