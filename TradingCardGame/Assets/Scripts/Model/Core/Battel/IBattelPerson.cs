using System;
using System.Collections.Generic;

public interface IBattelPerson
{
    event Action<byte> SetLive;
    event Action<bool> SetFortune;
    string Name { get; }
    IFraction Fraction { get; }
    TypePersonEnum TypePerson { get; }
    List<ICardData> DeckCards { get; set; }
    List<IAttackCard> ReservCards { get; }
    List<IAttackCard> AttackCards { get; }
    List<ICellBattel> Cell { get; set; }

    void AssingCells(List<ICellBattel> cell);
    bool Fortune { get; set; }
    string Report { get; set; }
    bool IsReadyContinue { get; set; }
    byte Live { get; set; }
    IBattelPerson EnemyPerson { get; set; }

    void Creat(string name, string fraction, List<string> cards, byte live = 10);
    void NewStartingHand();
    void BringCardsToBattlefield(float animationTime, Action actEndRelocation);
    void ShuffleCards(List<ICardData> cards);
    void PlaceAttackCell(IAttackCard card, ICellBattel cell, Action finish = null);
}
