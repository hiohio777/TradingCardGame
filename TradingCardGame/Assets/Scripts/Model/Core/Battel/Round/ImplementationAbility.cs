using System;

public class ImplementationAbility : QueueHendler
{
    public ImplementationAbility(IBattelBase battel, Action actionFinish) : base(battel, actionFinish) =>
        triggerAbility = EventTriggerEnum.Implementation;

    sealed protected override void Finish() => actionFinish.Invoke();
}
