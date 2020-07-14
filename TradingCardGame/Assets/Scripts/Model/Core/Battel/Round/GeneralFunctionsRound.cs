﻿using System.Collections.Generic;
using System.Linq;

public static class GeneralFunctionsExtension
{
    public static List<IAttackCard> GetAllAttackCards(this IBattelBase battel)
    {
        var cardsQueue = new List<IAttackCard>(battel.Player.AttackCards);
        cardsQueue.AddRange(battel.Enemy.AttackCards);
        return cardsQueue;
    }
}
