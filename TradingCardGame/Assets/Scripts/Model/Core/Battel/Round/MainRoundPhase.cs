using System;

public class MainRoundPhase : QueueHendler
{
    public MainRoundPhase(IBattelBase battel, Action actionFinish) : base(battel, actionFinish)
    { }

    protected override void Next(IAttackCard current)
    {
        IAttackCard target;
        if (current.AttackTarget == -1 || (target = current.EnemyPerson.Cell[current.AttackTarget].Unit) == null
        || target.Combat.Class.Type != ClassCardEnum.sunset)
            new CommonActionsPhase(battel, RunQueue, cards).Run(current);
        else new SunSetActionsPhase(battel, RunQueue, cards).Run(current);
    }

    sealed protected override void Finish() => new EndRoundAbilityPhase(battel, actionFinish).Execute();
}