using System;
using System.Collections;

public class StartRoundAbilityPhase : QueueHendler
{
    public StartRoundAbilityPhase(IBattelBase battel, Action actionFinish) : base(battel, actionFinish) =>
        triggerAbility = EventTriggerEnum.StartRound;

    sealed protected override void Finish() => new MainRoundPhase(battel, actionFinish).Execute();
}