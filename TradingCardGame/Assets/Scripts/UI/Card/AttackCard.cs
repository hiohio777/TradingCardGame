using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackCard : Card, IAttackCard, IPointerClickHandler
{
    private Action<IAttackCard> click;

    public ITokensPanel Tokens => tokensPanel;
    public IAbility Ability { get; private set; }
    public WarriorCards Warrior { get; private set; }

    public ICombatCard Combat => Warrior.Combat;
    public int Id => Warrior.Id;

    public void Build(ICardData cardData, Vector3 scale,
    ICombatCard combat, IAbility ability)
    {
        Build(cardData, scale);
        Warrior = new WarriorCards(Moving, View, combat);
        Ability = ability;
    }

    public void SetClickListener(Action<IAttackCard> click) => this.click = click;
    public override void ClearClickListener() => click = null;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (Moving.IsMoving == false)
        {
            click?.Invoke(this);
        }
    }

    // Карта совершает анимацию и показывает специфект способности
    public void ExecuteAbility(EventTriggerEnum trigger, IBattelBase battel, Action finish)
    {
        View.Frame(true);
        Ability.TriggerEvent(trigger, this, battel, () => { View.Frame(false); finish?.Invoke(); });
    }

    public void ImplementAbility(TypeSpecificityEnum specificity, Action finish = null)
    {
        StartSFX(specificity);
        Action act = () => Moving.SetScale(1).SetWaitTime(0.5f, 0.5f).Run(0.1f, finish);
        Moving.SetScale(1.1f).Run(0.1f, act);
    }

    #region Death
    public void Death(Action finash)
    {
        Action die = () =>
        {
            Warrior.FriendPerson.Live -= 1;
            Destroy();
            finash.Invoke();
        };

        View.SetSortingOrder(300);
        Moving.SetPosition(new Vector3(-1000, 80)).SetRotation(300).Run(0.5f, die);
    }
    public void Destroy()
    {
        Warrior.Cell.Unit = null;
        Ability.Destroy();
        DestroyUI();
    }
    #endregion

    #region Warrior
    public void AddAttacker(IAttackCard current) => Warrior.AddAttacker(current);
    public void RemoveAttacker() => Warrior.RemoveAttacker();
    public void PlaceAttackCell(ICellBattel cell, bool isMoving = true, Action finish = null)
    {
        Warrior.Cell = cell;
        cell.Unit = this;
        Action finishTemp = () =>
        {
            View.SetScale(new Vector3(1, 1, 1)).Frame(false);
            finish?.Invoke();
        };
        Moving.SetPosition(Warrior.Cell.Position).SetRotation(0);
        if (isMoving)
            Moving.Run(0.3f, finishTemp);
    }
    #endregion
}
