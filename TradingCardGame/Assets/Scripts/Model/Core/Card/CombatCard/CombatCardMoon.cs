using System;
using UnityEngine;

public class CombatCardMoon : BaseCombatCard, ICombatCard
{
    public CombatCardMoon(ICardUIParameters parameters, ICardData cardData, Action<TypeSpecificityEnum> startSpecificity)
        : base(parameters, cardData, startSpecificity)
        { }

    public override bool StandardDamage(IAttackCard enemy)
    {
        if (enemy.Combat.Attack < 5)
        {
            startSpecificity.Invoke(TypeSpecificityEnum.MoonShield);
            return false;
        }
        return base.StandardDamage(enemy);
    }
}
