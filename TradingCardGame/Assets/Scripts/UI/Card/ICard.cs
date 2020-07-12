using System;
using UnityEngine;

public interface ICardBaseUI
{
    Vector3 Posotion { get; }
    ICard SetSortingOrder(int sortingOrder);
    ICard SetOldSortingOrder();
    ICard SetScale(Vector3 scale);
    ICard SetRotation(float rotation);
    ICard SetPosition(Vector2 position);
    IMovingCard Moving { get; }
    Transform TransformCard { get; }
    void Frame(bool isSelected);
    void Frame(bool isSelected, Color color);
    void ClearClickListener();
    void MoveTo(float time, Vector3 target, Action execute = null, float waitTime = 0);
    void StopMoveing();
    void StartSpecificity(TypeSpecificityEnum type);
    void StartSpecificity(TypeSpecificityEnum type, Action actFinish);
    void StopSpecificity();
    void Destroy();
}

public interface ICard : ICardBaseUI
{
    ICardData CardData { get; }
    ICard SetParent(Transform parent);
    Vector2 Size { get; }
    StatusCardEnum Status { get; set; }
    void SetClickListener(Action<ICard> click);
}

public interface IBattelCard : ICardBaseUI
{
    ICombatCard Combat { get; }
    ITokensPanel Tokens { get; }
    IAbility Ability { get; }

    void SetClickListener(Action<IBattelCard> click);
    void SetClickListener(Action<IAttackCard> clickAttackCard);
}
