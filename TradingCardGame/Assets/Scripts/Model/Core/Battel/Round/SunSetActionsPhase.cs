using System;
using System.Collections.Generic;

public class SunSetActionsPhase : RoundActionsPhase
{
    public SunSetActionsPhase(IBattelBase battel, Action<IAttackCard> actionFinish, List<IAttackCard> cards) : base(battel, actionFinish, cards)
    { }

    protected override void ProceedAccordingCardClass() => DefenseDamage();
    protected override void LaunchDefense() => CompletionMove();
    protected override void LaunchAttack() => CheckBeforeAttack();
}
