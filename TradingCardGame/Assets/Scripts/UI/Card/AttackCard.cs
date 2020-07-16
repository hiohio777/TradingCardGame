using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackCard : Card, IAttackCard, IPointerClickHandler
{
    private Action<IAttackCard> click;

    public ITokensPanel Tokens { get; }
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

    public void ReturnCardToPlace(Action execute = null, float time = 0.3f)
    {
        View.SetSortingOrder(Warrior.Id);
        Moving.SetPosition(View.Position).Run(0.3f, () => { View.SetSortingOrder(0); execute.Invoke(); });
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
        View.SetSortingOrder(300);

        Action die = () =>
        {
            Warrior.FriendPerson.Live -= 1;
            Destroy();
            finash.Invoke();
        };

        var deathEndPosition = new Vector3(-1000, 80);
        Moving.SetPosition(deathEndPosition).SetRotation(300).Run(0.5f, die);
    }
    public override void Destroy()
    {
        Warrior.Cell.Unit = null;
        Warrior = null;
        base.Destroy();
    }
    #endregion

    #region Warrior
    public void AddAttacker(IAttackCard current) => Warrior.AddAttacker(current);
    public void RemoveAttacker() => Warrior.RemoveAttacker();
    public void PlaceAttackCell(ICellBattel cell, Action finish)
        => Warrior.PlaceAttackCell(cell, finish);
    #endregion
}
