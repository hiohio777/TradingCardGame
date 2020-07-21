using System;

public interface IAttackCard
{
    int Id { get; }
    CardBase View { get; }
    IMovingCard Moving { get; }

    WarriorCards Warrior { get; }
    ICombatCard Combat { get; }
    ITokensPanel Tokens { get; }
    IAbility Ability { get; }

    void ImplementAbility(TypeSpecificityEnum specificity, Action finish);
    void ExecuteAbility(EventTriggerEnum trigger, IBattelBase battel, Action finish = null);
    void StartSFX(TypeSpecificityEnum type, Action finish = null);

    void SetClickListener(Action<IAttackCard> clickAttackCard);
    void ClearClickListener();

    void PlaceAttackCell(ICellBattel cell, bool isMoving = true, Action finish = null);
    void AddAttacker(IAttackCard current);
    void RemoveAttacker();

    void Death(Action finish);
    void DestroyUI();
    void Destroy();
}