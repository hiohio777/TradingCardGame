﻿using System;

public class CombatCardSun : BaseCombatCard, ICombatCard
{
    public CombatCardSun(ICardUIParameters parameters, ICardData cardData, Action<TypeSpecificityEnum> startSpecificity)
        : base(parameters, cardData, startSpecificity)
    { }

    public override bool StandardDamage(IAttackCard enemy)
    {
        if (IsDownAuraBroken() == false) return false;
        return base.StandardDamage(enemy);
    }

    public override bool DefenseDamage(IAttackCard enemy)
    {
        if (IsDownAuraBroken() == false) return false;
        return base.DefenseDamage(enemy);
    }

    private bool IsDownAuraBroken()
    {
        if (ClassAura)
        {
            ClassAura = false;
            startSpecificity.Invoke(TypeSpecificityEnum.SunAuraBroken);
            return false;
        }

        return true;
    }
}
