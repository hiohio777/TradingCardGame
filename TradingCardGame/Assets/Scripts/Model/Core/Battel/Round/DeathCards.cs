using System;
using System.Collections.Generic;

public class DeathCards : QueueHendler
{
    private readonly List<IAttackCard> attackCards;

    public DeathCards(IBattelBase battel, Action actionFinish, List<IAttackCard> attackCards)
        : base(battel, actionFinish) =>
        this.attackCards = attackCards ?? new List<IAttackCard>();

    public override void Execute()
    {
        cards = battel.CombatData.ResponsekDamageCards;
        RunQueue();
    }

    protected override void Next(IAttackCard current)
    {
        var health = current.Combat.Health;
        if (health > 0)
            current.ExecuteAbility(EventTriggerEnum.ResponsekDamage, battel, () => { RunQueue(current); });
        else if (health == 0) FinalDeathCard(current);
        else DeathAbility(current);
    }

    protected override void Finish() => actionFinish.Invoke();

    private void DeathAbility(IAttackCard current) =>
        current.ExecuteAbility(EventTriggerEnum.Death, battel, () => { FinalDeathCard(current); });

    private void FinalDeathCard(IAttackCard current)
    {
        if (current.Combat.Health <= 0)
        {
            // Карта умирает
            attackCards.Remove(current);
            current.FriendPerson.AttackCards.Remove(current);

            current.Death(() => ContinueRound(current));
        }
        else RunQueue(current);
    }

    private void ContinueRound(IAttackCard current)
    {
        if (current.FriendPerson.Live > 0)
        {
            RunQueue(current);
        }
        else
        {
            battel.Winner = current.EnemyPerson.TypePerson;
            battel.OnFinishBattel();
        }
    }
}
