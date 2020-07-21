using System;
using UnityEngine;

public interface ICard
{
    CardBase View { get; }
    IMovingCard Moving { get; }
    StatusCardEnum Status { get; set; }
    ICardData CardData { get; }

    void SetClickListener(Action<ICard> click);
    void ClearClickListener();

    void StartSFX(TypeSpecificityEnum type, Action actFinish);
    void DestroyUI();
    void Build(ICardData cardData, Vector3 scale);
}