using System.Collections.Generic;
using System.Linq;

public static class GeneralFunctionsRound
{
    public static List<IAttackCard> CreatQueue(IBattelBase battel)
    {
        var cardsQueue = new List<IAttackCard>(battel.Player.AttackCards);
        cardsQueue.AddRange(battel.Enemy.AttackCards);
        return cardsQueue.Where(x => x.BattelCard != null).ToList(); //Искючить погибшие карты
    }
}
