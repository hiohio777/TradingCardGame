﻿using System;

public class CombatCardСommon : BaseCombatCard, ICombatCard
{
    public CombatCardСommon(ICardUIParameters parameters, ICardData cardData, Action<TypeSpecificityEnum> startSpecificity)
        : base(parameters, cardData, startSpecificity)
    { }
}
