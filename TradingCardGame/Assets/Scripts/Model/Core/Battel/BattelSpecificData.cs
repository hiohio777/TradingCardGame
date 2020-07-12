using System;

public class BattelSpecificData : IBattelSpecific
{
    public event Action<byte> SetRounds;
    public event Action<byte> SetCardResetCounter;

    public byte Rounds { get => rounds; set { rounds = value; SetRounds?.Invoke(rounds); } }
    public byte CardResetCounter { get => cardResetCounter; set { cardResetCounter = value; SetCardResetCounter?.Invoke(cardResetCounter); } }

    private byte rounds;
    private byte cardResetCounter;
}
