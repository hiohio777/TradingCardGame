using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICardBase
{
    ICardData CardData { get; }
    ICardUIBase View { get; }
    IMovingCard Moving { get; }
    void MoveTo(float time, Vector3 target, Action execute = null, float waitTime = 0);
    void StartSpecificity(TypeSpecificityEnum type, Action actFinish);
    void ClearClickListener();
    void Destroy();
}

public interface ICard : ICardBase
{
    StatusCardEnum Status { get; set; }
    void SetClickListener(Action<ICard> click);
}

public interface IAttackCard : ICardBase
{
    int Id { get; }
    Vector3 DefaultPosition { get; }
    bool Fortune { get; }

    ICombatCard Combat { get; }
    ITokensPanel Tokens { get; }
    IAbility Ability { get; }

    ICellBattel Cell { get; set; }
    List<IAttackCard> Enemies { get; set; }
    int AttackTarget { get; set; }

    IBattelPerson FriendPerson { get; }
    IBattelPerson EnemyPerson { get; }
    TypePersonEnum TypePerson { get; }

    void ImplementAbility(TypeSpecificityEnum specificity, Action finish);
    void SetClickListener(Action<IAttackCard> clickAttackCard);

    void AddAttacker(IAttackCard attacker);
    void RemoveAttacker(IAttackCard attacker);
    void ReturnCardToPlace(Action execute = null, float time = 0.3f);
    void Build(IBattelPerson enemy, IBattelPerson player, TypePersonEnum typePerson);
    void Death(Action finish);


    void PlaceAttackCell(ICellBattel cell, TypePersonEnum typePerson, Action finish = null);
    void ExecuteAbility(EventTriggerEnum trigger, IBattelBase battel, Action finish);
}