using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class PersonDATAREPORT
{
    public List<string> cardsAttack, cardReserv;

    public PersonDATAREPORT(List<string> cardsAttack, List<string> cardReserv)
    {
        this.cardsAttack = cardsAttack ?? throw new ArgumentNullException(nameof(cardsAttack));
        this.cardReserv = cardReserv ?? throw new ArgumentNullException(nameof(cardReserv));
    }

    public string GetJsonString() => JsonConvert.SerializeObject(this);
}

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