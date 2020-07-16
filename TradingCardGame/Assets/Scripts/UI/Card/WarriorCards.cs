using System;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCards
{
    private static int x1 = 30, x2 = 50, y1 = 110, y2 = 150;
    public IMovingCard Moving;
    public CardBase View;

    public WarriorCards(IMovingCard moving, CardBase cardBase, ICombatCard combat)
    {
        (this.Moving, this.View, Combat) = (moving, cardBase, combat);
        Enemies = new List<WarriorCards>();
    }

    public ICombatCard Combat { get; private set; }
    public TypePersonEnum TypePerson => FriendPerson.TypePerson;
    public IBattelPerson FriendPerson { get; set; }
    public IBattelPerson EnemyPerson { get; set; }
    public List<WarriorCards> Enemies { get; set; } = new List<WarriorCards>();
    public ICellBattel Cell { get; set; }
    public int Id => Cell.Id;

    public int AttackTargetID { get; set; } = -1;
    public IAttackCard AttackTargetUnit =>
        AttackTargetID == -1 ? null : EnemyPerson.Cell[AttackTargetID].Unit;

    public void Clear()
    {
        Enemies.Clear();
        AttackTargetID = -1;
    }
    #region Assign to attack
    public void AddAttacker(IAttackCard attacker)
    {
        if (Enemies.Count >= Combat.MaxCountAttackers)
            return; // Уже атакует максимум врагов

        Action<int> Final = (x) => attacker.View.Frame(false).SetSortingOrder(x);
        if (Enemies.Count > 0)
        {
            if (Enemies[0].Combat.Initiative > attacker.Combat.Initiative)
            {
                // Поменять атакующие карты местами
                attacker.Moving.SetPosition(new Vector3(View.Position.x + x1, View.Position.y - y1, 0)).Run(0.3f, () => Final(1));
                Enemies[0].Moving.SetPosition(new Vector3(View.Position.x + x2, View.Position.y - y2, 0)).Run(0.3f, () => Enemies[0].View.SetSortingOrder(2));
            }
            else
            {
                // Назначить второй атакующей
                attacker.Moving.SetPosition(new Vector3(View.Position.x + x2, View.Position.y - y2, 0)).Run(0.3f, () => Final(0));
            }
        }
        else
        {
            // Назначить первой атакующей
            attacker.Moving.SetPosition(new Vector3(View.Position.x + x1, View.Position.y - y1, 0)).Run(0.3f, () => Final(1));
        }

        Enemies.Add(attacker.Warrior);
        attacker.Warrior.AttackTargetID = Id;
    }

    public void RemoveAttacker()
    {
        var enemies = EnemyPerson.Cell[AttackTargetID].Unit.Warrior.Enemies;
        enemies.Remove(this);

        if (enemies.Count > 0)
        {
            var pos = EnemyPerson.Cell[AttackTargetID].Position;
            enemies[0].View.SetSortingOrder(1);
            enemies[0].Moving.SetPosition(new Vector3(pos.x + x1, pos.y - y1, 0)).Run(0.3f);
        }

        AttackTargetID = -1;
        View.Frame(true);
        Moving.SetPosition(Cell.Position).Run(0.3f, () => View.Frame(false).SetSortingOrder(0));
    }
    #endregion

    // Карта выходит на поле боя в ячеку с idCell
    public void PlaceAttackCell(ICellBattel cell, Action finish)
    {
        Cell = cell;
        Action finishTemp = () =>
        {
            View.SetScale(new Vector3(1, 1, 1)).Frame(false);
            finish.Invoke();
        };
        Moving.SetPosition(Cell.Position).Run(0.3f, finishTemp);
    }
}
