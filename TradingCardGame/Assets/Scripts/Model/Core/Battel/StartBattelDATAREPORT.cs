using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class StartBattelDATAREPORT
{
    public string name;
    public string fraction;
    public List<string> cards;

    public StartBattelDATAREPORT(string name, string fraction, List<string> cards)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.fraction = fraction ?? throw new ArgumentNullException(nameof(fraction));
        this.cards = cards ?? throw new ArgumentNullException(nameof(cards));
    }

    public string GetJsonString() => JsonConvert.SerializeObject(this);
}