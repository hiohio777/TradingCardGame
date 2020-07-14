using System.Collections.Generic;
using UnityEngine;

public class Health_Conditions : StandartBaseConditions, IPostConditions, IStartingConditions
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, List<IAttackCard> cardsTarget, IBattelBase battel)
    {
        int i = 0;
        while (i < cardsTarget.Count)
        {
            if (SetResult(cardsTarget[i].Combat.Health) == false) cardsTarget.RemoveAt(i);
            else i++;
        }

        return cardsTarget;
    }

    public bool IsConditions(IAttackCard card, List<IAttackCard> cards, IBattelBase battel) =>
                      GetTargetCards(card, cards, battel).Count > 0;
}

