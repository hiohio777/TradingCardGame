using System;

public interface ICardResetCounterUI
{
    event Action<int> Strengthen;
    event Action Clear;
    event Action ResetCards;
}
