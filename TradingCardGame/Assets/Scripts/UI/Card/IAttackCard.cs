using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackCard
{
    int Id { get; }
    CardBase View { get; }
    IMovingCard Moving { get; }

    ICombatCard Combat { get; }
    ITokensPanel Tokens { get; }
    IAbility Ability { get; }
    WarriorCards Warrior { get; }

    void ImplementAbility(TypeSpecificityEnum specificity, Action finish);
    void ExecuteAbility(EventTriggerEnum trigger, IBattelBase battel, Action finish = null);
    void StartSFX(TypeSpecificityEnum type, Action finish = null);

    void SetClickListener(Action<IAttackCard> clickAttackCard);
    void ClearClickListener();

    void PlaceAttackCell(ICellBattel cell, Action finish);
    void AddAttacker(IAttackCard current);
    void RemoveAttacker();

    void Death(Action finish);
    void Destroy();
}