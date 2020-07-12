using System;
using System.Collections.Generic;

public class CommonActionsPhase : RoundActionsPhase
{
    public CommonActionsPhase(IBattelBase battel, Action<IAttackCard> actionFinish, List<IAttackCard> cards) : base(battel, actionFinish, cards)
    { }

    protected override void ProceedAccordingCardClass() => CheckBeforeAttack();
    protected override void LaunchDefense() => DefenseDamage();
    protected override void LaunchAttack() => CompletionMove();
}
