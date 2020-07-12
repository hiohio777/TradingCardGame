using System;
using System.Collections.Generic;

public interface IBattelPerson
{
    event Action<byte> SetLive;
    event Action<bool> SetFortune;
    string Name { get; }
    IFraction Fraction { get; }
    List<IBattelCard> ReservCards { get; }
    List<IAttackCard> AttackCards { get; }
    List<ICardData> DeckCards { get; set; }
    bool Fortune { get; set; }
    string Report { get; set; }
    bool IsReadyContinue { get; set; }
    byte Live { get; set; }
    void Creat(string name, string fraction, List<string> cards, byte live = 10);
    void NewStartingHand();
    void MoveToReservLocation(Action action = null, float yPosition = -450, int offset = 140);
    void SetAttackCardsPosition(List<IAttackCard> CardsPosition);
    void BringCardsToBattlefield(float animationTime, Action actEndRelocation);
    void ReturnCardToPlace(Action execute = null);
    void ReturnCardsToDeck();
}
