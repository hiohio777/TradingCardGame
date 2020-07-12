using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackCard
{
    int Id { get; }
    List<IAttackCard> Enemies { get; set; }
    IAttackCard AttackCardTarget { get; set; }

    IBattelPerson FriendPerson { get; }
    IBattelPerson EnemyPerson { get; }
    TypePersonEnum TypePerson { get; }
    bool Fortune { get; }
    void PutCardFromReserve(IBattelCard battelCard);
    IBattelCard Pop();
    Vector3 DefaultPosition { get; }
    Vector3 CurrentPosotion { get; }
    void SetClickListener(Action<IAttackCard> click);
    void ClearClickListener();
    void AddAttacker(IAttackCard attacker);
    void RemoveAttacker(IAttackCard attacker);
    void ReturnCardToPlace(Action execute = null, float time = 0.3f);
    void Build(IBattelPerson enemy, IBattelPerson player, TypePersonEnum typePerson);
    void Death(Action act);

    IBattelCard BattelCard { get; }
    void ExecuteAbility(EventTriggerEnum trigger, IBattelBase battel, Action finish);
}
