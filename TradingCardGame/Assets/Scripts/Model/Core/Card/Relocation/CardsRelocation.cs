using System;
using System.Collections.Generic;

public class CardsRelocation : ActionModule
{
    public CardsRelocation(List<IAttackCard> cards, Action actNext, float time, float waitTime = 0, float waitAfterTime = 0)
        : base(actNext, cards.Count) =>
        cards.ForEach(x => x.BattelCard.Moving.SetWaitTime(waitTime, waitAfterTime).Run(time, Final));

    public CardsRelocation(List<IBattelCard> cards, Action actNext, float time, float waitTime = 0, float waitAfterTime = 0)
        : base(actNext, cards.Count) =>
        cards.ForEach(x => x.Moving.SetWaitTime(waitTime, waitAfterTime).Run(time, Final));
}