using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : IAttackCard
{
    public Vector3 DefaultPosition => Cell != null ? Cell.Position : Posotion;
    public Vector3 Posotion { get; }

    public ICellBattel Cell { get; set; }
    public int Id => Cell.Id;

    public TypePersonEnum TypePerson { get; private set; }
    public IBattelPerson FriendPerson { get; private set; }
    public IBattelPerson EnemyPerson { get; private set; }
    public bool Fortune { get => FriendPerson.Fortune; }

    private Action<IAttackCard> click;

    public void Build(IBattelPerson friend, IBattelPerson enemy, TypePersonEnum typePerson) =>
    (FriendPerson, EnemyPerson, TypePerson) = (friend, enemy, typePerson);

    public void SetClickListener(Action<IAttackCard> click) => this.click = click;
    public void ClearClickListener() => click = null;

    public void ReturnCardToPlace(Action execute = null, float time = 0.3f)
    {
        SetSortingOrder(Cell.Id);
        MoveTo(time, DefaultPosition, execute: () => { SetSortingOrder(0); execute.Invoke(); SetSortingOrder(Cell.Id); });
    }

    public List<IAttackCard> Enemies { get; set; } = new List<IAttackCard>();
    public int AttackTarget { get; set; } = -1;
    private readonly int x1 = 30, x2 = 50, y1 = 110, y2 = 150;
    public void AddAttacker(IAttackCard newAttacker)
    {

        if (Enemies.Count > 0)
        {
            if (Enemies[0].Combat.Initiative > newAttacker.Combat.Initiative)
            {
                // Поменять атакующие карты местами
                Action actFinish = () => { newAttacker.Frame(false); newAttacker.SetSortingOrder(1); };
                newAttacker.MoveTo(0.3f, new Vector3(DefaultPosition.x + x1, DefaultPosition.y - y1, 0), execute: actFinish);
                Enemies[0].SetSortingOrder(2);
                Enemies[0].MoveTo(0.3f, new Vector3(DefaultPosition.x + x2, DefaultPosition.y - y2, 0));
            }
            else
            {
                // Назначить второй атакующей
                Action actFinish = () => { newAttacker.Frame(false); newAttacker.SetSortingOrder(2); };
                newAttacker.MoveTo(0.3f, new Vector3(DefaultPosition.x + x2, DefaultPosition.y - y2, 0), execute: actFinish);
            }
        }
        else
        {
            // Назначить первой атакующей
            Action actFinish = () => { newAttacker.Frame(false); newAttacker.SetSortingOrder(1); };
            newAttacker.MoveTo(0.3f, new Vector3(DefaultPosition.x + x1, DefaultPosition.y - y1, 0), execute: actFinish);
        }

        Enemies.Add(newAttacker);
        newAttacker.AttackTarget = this.Id;
    }

    public void RemoveAttacker(IAttackCard attacker)
    {
        Enemies.Remove(attacker);

        if (Enemies.Count > 0)
            Enemies[0].SetSortingOrder(1).MoveTo(0.3f, new Vector3(DefaultPosition.x + x1, DefaultPosition.y - y1, 0));

        attacker.AttackTarget = -1;

        attacker.Frame(true);
        Action actFinish = () => { attacker.Frame(false); attacker.SetSortingOrder(0); };
        attacker.MoveTo(0.3f, attacker.DefaultPosition, execute: actFinish);
    }

    public void ExecuteAbility(EventTriggerEnum trigger, IBattelBase battel, Action finish)
    {
        Frame(true);
        Ability.TriggerEvent(trigger, this, battel, () => { Frame(false); finish?.Invoke(); });
    }

    public void ImplementAbility(TypeSpecificityEnum specificity, Action finish)
    {
        StartSpecificity(specificity);
        Action act = () => Moving.SetScale(1).SetWaitTime(0.5f, 0.5f).Run(0.1f, finish);
        Moving.SetScale(1.1f).Run(0.1f, act);
    }

    public void Death(Action finash)
    {
        StopSpecificity();
        SetSortingOrder(300);

        Action die = () =>
        {
            FriendPerson.Live -= 1;
            Destroy();
            finash.Invoke();
        };

        var deathEndPosition = new Vector3(-1000, 80);
        Moving.SetPosition(deathEndPosition).SetRotation(300).Run(0.5f, die);
    }

    public ICombatCard Combat { get; }
    public ITokensPanel Tokens { get; }
    public IAbility Ability { get; }
    public IMovingCard Moving { get; }

    #region MyRegion
    // Карта выходит на поле боя в ячеку с idCell
    public void PlaceAttackCell(ICellBattel cell, TypePersonEnum typePerson, Action finish)
    {
        (Cell, Cell.Unit, TypePerson) = (cell, this, typePerson);

        Action finishTemp = () =>
        {
            SetScale(new Vector3(1, 1, 1)).Frame(false);
            finish.Invoke();
        };
        Moving.SetPosition(Cell.Position).Run(0.3f, finishTemp);
    }

    public void Destroy()
    {
        Cell.Unit = null;
        Enemies.Clear();
        AttackTarget = -1;
    }
    #endregion

    public ICard SetSortingOrder(int sortingOrder)
    {
        throw new NotImplementedException();
    }

    public ICard SetOldSortingOrder()
    {
        throw new NotImplementedException();
    }

    public ICard SetScale(Vector3 scale)
    {
        throw new NotImplementedException();
    }

    public ICard SetRotation(float rotation)
    {
        throw new NotImplementedException();
    }

    public ICard SetPosition(Vector2 position)
    {
        throw new NotImplementedException();
    }

    public Transform TransformCard { get; }

    public void Frame(bool isSelected)
    {
        throw new NotImplementedException();
    }

    public void Frame(bool isSelected, Color color)
    {
        throw new NotImplementedException();
    }

    public void MoveTo(float time, Vector3 target, Action execute = null, float waitTime = 0)
    {
        throw new NotImplementedException();
    }

    public void StopMoveing()
    {
        throw new NotImplementedException();
    }

    public void StartSpecificity(TypeSpecificityEnum type)
    {
        throw new NotImplementedException();
    }

    public void StartSpecificity(TypeSpecificityEnum type, Action actFinish)
    {
        throw new NotImplementedException();
    }

    public void StopSpecificity()
    {
        throw new NotImplementedException();
    }
}
