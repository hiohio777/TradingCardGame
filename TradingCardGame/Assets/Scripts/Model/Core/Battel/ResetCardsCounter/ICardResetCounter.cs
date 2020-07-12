using System;

public interface ICardResetCounter
{
    void OnStrengthen(IBattelBase battel, Action finish);
    void OnClear();
}