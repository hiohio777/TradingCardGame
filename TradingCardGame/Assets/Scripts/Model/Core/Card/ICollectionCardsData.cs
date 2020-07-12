using System.Collections.Generic;

public interface ICollectionCardsData
{
    Dictionary<string, ICardData> Cards { get; }
    List<ICardData> GetFractionCards(string fraction, bool isOnlyOpenCards = false);
    List<ICardData> GetCards(List<string> names);
    ICardData GetCard(string name);
    void OpenCard(string name);
}
