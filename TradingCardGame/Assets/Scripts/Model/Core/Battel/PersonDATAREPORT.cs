using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
