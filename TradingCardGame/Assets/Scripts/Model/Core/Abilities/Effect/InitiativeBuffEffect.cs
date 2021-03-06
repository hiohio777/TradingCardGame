﻿using System.Collections.Generic;
using UnityEngine;

public class InitiativeBuffEffect : StandartBuffEffect, IEffect
{
    [SerializeField, Range(-8, 8)] private sbyte initiative = 0;

    public bool Execute(IAttackCard card, List<IAttackCard> cardsTarget, IBattelBase battel, ISFXFactory specificityFactory) =>
        Execute(cardsTarget);
    protected override bool Buff(IAttackCard card) =>
        card.Combat.BuffInitiative(initiative);
}
