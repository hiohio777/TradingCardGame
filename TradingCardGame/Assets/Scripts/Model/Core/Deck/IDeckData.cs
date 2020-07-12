using System.Collections.Generic;

public interface IDeckData
{
    string Name { get; set; }
    string Fraction { get; set; }
    StatusDeckEnum Status { get; set; }
    List<string> StringCards { get; set; }
    List<ICardData> Cards { get; set; }
    bool AddCard(ICardData cardData);
    bool RemoveCard(ICardData cardData);
    bool IsInitiativeCards(ICardData card);
    IDeckData Clone();
    void PutClon(IDeckData clon);
}