using System.Collections.Generic;
using UnityEngine;

public class DefenseBuffEffect : StandartBuffEffect, IEffect
{
    [SerializeField, Range(-30, 30)] private sbyte defense = 0;

    public bool Execute(IAttackCard card, List<IAttackCard> cardsTarget, IBattelBase battel, ISFXFactory specificityFactory) =>
        Execute(cardsTarget);
    protected override bool Buff(IAttackCard card) =>
        card.Combat.BuffDefense(defense);
}
