using System;
using System.Collections.Generic;

public class ReturnCardToPlaceModule : ActionModule
{
    public ReturnCardToPlaceModule(List<IAttackCard> cards, Action actEndRelocation = null) : base(actEndRelocation, cards.Count)
    {
        cards.ForEach(x => x.ReturnCardToPlace(Final));
    }
}
