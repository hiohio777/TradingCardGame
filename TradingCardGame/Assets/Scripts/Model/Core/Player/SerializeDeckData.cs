using System;
using System.Collections.Generic;

public class SerializeDeckData
{
    public string name;
    public string fraction;
    public List<string> stringCards;

    public SerializeDeckData(string name, string fraction, List<string> stringCards)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.fraction = fraction ?? throw new ArgumentNullException(nameof(fraction));
        this.stringCards = stringCards ?? throw new ArgumentNullException(nameof(stringCards));
    }
}
