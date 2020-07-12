using System.Collections.Generic;
using UnityEngine;

public interface ICardFactory<T>
{
    void ClearBuffer();
    T GetCard(ICardData cardData);
    T GetCard(ICardData cardData, Vector3 scale);
    List<T> GetCards(List<ICardData> cardsData, Vector3 scale);
    List<T> GetCards(List<ICardData> cardsData);
}
