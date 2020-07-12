using System;
using System.Collections.Generic;

public interface ILocation
{
    void PutCards(List<IBattelCard> cards, Action final);
}
