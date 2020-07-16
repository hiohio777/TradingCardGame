using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundActionsPhase
{
    private readonly List<IAttackCard> cards;
    private readonly IBattelBase battel;
    private readonly Action<IAttackCard> actionFinish;
    private IAttackCard current;
    private IAttackCard CardTarget => current.Warrior.AttackTargetUnit;
    private readonly float timeMoveCard = 0.5f, timeHit = 0.2f, waitTime = 0.4f;

    protected RoundActionsPhase(IBattelBase battel, Action<IAttackCard> actionFinish, List<IAttackCard> cards)
    {
        (this.battel, this.actionFinish, this.cards) = (battel, actionFinish, cards);
    }

    public void Run(IAttackCard current)
    {
        this.current = current;
        current.View.Frame(true);
        ApplyAbility(EventTriggerEnum.LeadUp, current, MoveToAttack);
    }

    private void MoveToAttack()
    {
        if (CardTarget != null)
        {
            int offset = current.Warrior.TypePerson == TypePersonEnum.player ? -160 : 160;
            var targetPosition = new Vector3(CardTarget.Warrior.Cell.Position.x + 10, CardTarget.Warrior.Cell.Position.y + offset);
            current.Moving.SetPosition(targetPosition).SetWaitTime(waitTime).Run(timeMoveCard, ProceedAccordingCardClass);
        }
        else ApplyFinalAbility();
    }

    protected abstract void ProceedAccordingCardClass();

    protected void CheckBeforeAttack()
    {
        if (CardTarget == null) CompletionMove();
        else ApplyAbility(EventTriggerEnum.BeforeAttack, current, AttackDamage);
    }

    private void AttackDamage()
    {
        int offset = current.Warrior.TypePerson == TypePersonEnum.player ? 20 : -20;
        var targetPosition = new Vector3(current.View.Position.x, current.View.Position.y + offset);
        current.Moving.SetPosition(targetPosition).SetWaitTime(waitTime).Run(timeHit, () => EndHitAttackDamage(offset));
    }

    private void EndHitAttackDamage(int offset)
    {
        Action act = () => CheckForDeath(AfterAttackAbility);

        var targetPosition = new Vector3(current.View.Position.x, current.View.Position.y - offset);

        if (CardTarget.Combat.StandardDamage(current))
        {
            current.Moving.SetPosition(targetPosition).Run(timeHit);
            CardTarget.StartSFX(TypeSpecificityEnum.StandardDamage, act);
        }
        else current.Moving.SetPosition(targetPosition).Run(timeHit, act);
    }

    private void AfterAttackAbility()
    {
        ApplyAbility(EventTriggerEnum.AfterAttack, current, LaunchDefense);
    }

    protected abstract void LaunchDefense();

    protected void DefenseDamage()
    {
        if (CardTarget == null || CardTarget.Combat.Defense == 0
            || current.Combat.Class.Type == ClassCardEnum.dawn)
        {
            LaunchAttack();
            return;
        }

        int offset = current.Warrior.TypePerson == TypePersonEnum.player ? -20 : 20;
        var targetPosition = new Vector3(CardTarget.View.Position.x, CardTarget.View.Position.y + offset);
        CardTarget.View.SetSortingOrder(250);
        CardTarget.Moving.SetPosition(targetPosition).SetWaitTime(waitTime).Run(timeHit, () => EndHitDefenseDamage(offset));
    }

    private void EndHitDefenseDamage(int offset)
    {
        Action act = () =>
        {
            CardTarget.View.SetSortingOrder(0);
            CheckForDeath(LaunchAttack);
        };

        var targetPosition = new Vector3(CardTarget.View.Position.x, CardTarget.View.Position.y - offset);
        if (current.Combat.DefenseDamage(CardTarget))
        {
            CardTarget.Moving.SetPosition(targetPosition).Run(timeHit);
            current.StartSFX(TypeSpecificityEnum.DefenseDamage, act);
        }
        else CardTarget.Moving.SetPosition(targetPosition).Run(timeHit, act);

    }

    protected abstract void LaunchAttack();

    protected void CompletionMove()
    {
        current.Moving.SetPosition(current.Warrior.Cell.Position).SetWaitTime(waitTime).Run(timeHit, ApplyFinalAbility);
    }

    private void ApplyFinalAbility()
    {
        Action act = () =>
        {
            current.View.Frame(false);
            actionFinish.Invoke(current);
        };

        ApplyAbility(EventTriggerEnum.FinalAbility, current, act);
    }

    private void ApplyAbility(EventTriggerEnum eventTrigger, IAttackCard current, Action nextAct) =>
        current.ExecuteAbility(eventTrigger, battel, () => CheckForDeath(nextAct));

    private void CheckForDeath(Action nextAct)
    {
        Action act = () =>
        {
            if (current != null) nextAct.Invoke();
            else actionFinish.Invoke(current);
        };

        new DeathCards(battel, act, cards).Execute();
    }
}
