using UnityEngine;

public interface ICardData : ICardDataBase
{
    Sprite Icon { get; }
    bool IsOpen { get; }
    GameObject LinkAbility { get; }
    TypeInitiativeEnum TypeInitiative { get; }
    ICardData Open();
}
