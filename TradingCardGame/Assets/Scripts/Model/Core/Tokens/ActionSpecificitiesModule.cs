using System;
using System.Collections.Generic;

public class ActionSpecificitiesModule : ActionModule
{
    public ActionSpecificitiesModule(TypeSpecificityEnum type, List<IAttackCard> cardsWithToken, Action actNext)
        : base(actNext, cardsWithToken.Count)
    {
        cardsWithToken.ForEach(x => x.StartSpecificity(type, Final));
    }
}