using System;

public class EndRoundAbilityPhase : QueueHendler
{
    public EndRoundAbilityPhase(IBattelBase battel, Action actionFinish) : base(battel, actionFinish) =>
        triggerAbility = EventTriggerEnum.EndRound;

    sealed protected override void Finish()
    {
        new TokenEndRound(battel, actionFinish).Execute();
    }
}
