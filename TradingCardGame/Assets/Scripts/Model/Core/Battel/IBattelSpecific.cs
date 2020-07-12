using System;

public interface IBattelSpecific
{
    event Action<byte> SetRounds;
    event Action<byte> SetCardResetCounter;

    byte Rounds { get; set; }
    byte CardResetCounter { get; set; }
}