using System;

public class BattleParameter
{
    public byte Item { get => item; set { item = value; act.Invoke(item); } }
    private byte item;
    private Action<byte> act;

    public BattleParameter(byte item, Action<byte> act)
    {
        this.item = item;
        this.act = act ?? throw new ArgumentNullException(nameof(act));
    }
}