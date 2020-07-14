using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundActionsPhase
{
    private readonly List<IAttackCard> cards;
    private readonly IBattelBase battel;
    private readonly Action<IAttackCard> actionFinish;
    private IAttackCard current;
    private IAttackCard CardTarget => current.EnemyPerson.Cell[current.AttackTarget].Unit;
    private readonly float timeMoveCard = 0.5f, timeHit = 0.2f, waitTime = 0.4f;

    protected RoundActionsPhase(IBattelBase battel, Action<IAttackCard> actionFinish, List<IAttackCard> cards)
    {
        (this.battel, this.actionFinish, this.cards) = (battel, actionFinish, cards);
    }

    public void Run(IAttackCard current)
    {
        this.current = current;
        current.Frame(true);
        ApplyAbility(EventTriggerEnum.LeadUp, current, MoveToAttack);
    }

    private void MoveToAttack()
    {
        if (CardTarget != null)
        {
            int offset = current.TypePerson == TypePersonEnum.player ? -160 : 160;
            var targetPosition = new Vector3(CardTarget.DefaultPosition.x + 10, CardTarget.DefaultPosition.y + offset);
            current.MoveTo(timeMoveCard, targetPosition, ProceedAccordingCardClass, waitTime);
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
        int offset = current.TypePerson == TypePersonEnum.player ? 20 : -20;
        var targetPosition = new Vector3(current.Posotion.x, current.Posotion.y + offset);
        current.MoveTo(timeHit, targetPosition, () => EndHitAttackDamage(offset), waitTime: waitTime);
    }

    private void EndHitAttackDamage(int offset)
    {
        Action act = () => CheckForDeath(AfterAttackAbility);

        var targetPosition = new Vector3(current.Posotion.x, current.Posotion.y - offset);

        if (CardTarget.Combat.StandardDamage(current))
        {
            current.MoveTo(timeHit, targetPosition);
            CardTarget.StartSpecificity(TypeSpecificityEnum.StandardDamage, act);
        }
        else current.MoveTo(timeHit, targetPosition, act);
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

        int offset = current.TypePerson == TypePersonEnum.player ? -20 : 20;
        var targetPosition = new Vector3(CardTarget.Posotion.x, CardTarget.Posotion.y + offset);
        CardTarget.SetSortingOrder(250);
        CardTarget.MoveTo(timeHit, targetPosition, () => EndHitDefenseDamage(offset), waitTime: waitTime);
    }

    private void EndHitDefenseDamage(int offset)
    {
        Action act = () =>
        {
            CardTarget.SetSortingOrder(0);
            CheckForDeath(LaunchAttack);
        };

        var targetPosition = new Vector3(CardTarget.Posotion.x, CardTarget.Posotion.y - offset);
        if (current.Combat.DefenseDamage(CardTarget))
        {
            CardTarget.MoveTo(timeHit, targetPosition);
            current.StartSpecificity(TypeSpecificityEnum.DefenseDamage, act);
        }
        else CardTarget.MoveTo(timeHit, targetPosition, act);

    }

    protected abstract void LaunchAttack();

    protected void CompletionMove()
    {
        current.MoveTo(timeHit, current.DefaultPosition, ApplyFinalAbility, waitTime: waitTime);
    }

    private void ApplyFinalAbility()
    {
        Action act = () =>
        {
            current.Frame(false);
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
