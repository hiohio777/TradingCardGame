using System;

public class MainRoundPhase : QueueHendler
{
    public MainRoundPhase(IBattelBase battel, Action actionFinish) : base(battel, actionFinish)
    { }

    protected override void Next(IAttackCard current)
    {
        if (current.Warrior.AttackTargetID == -1 || current.Warrior.AttackTargetUnit == null
        || current.Warrior.AttackTargetUnit.Combat.Class.Type != ClassCardEnum.sunset)
            new CommonActionsPhase(battel, RunQueue, cards).Run(current);
        else new SunSetActionsPhase(battel, RunQueue, cards).Run(current);
    }

    sealed protected override void Finish() => new EndRoundAbilityPhase(battel, actionFinish).Execute();
}