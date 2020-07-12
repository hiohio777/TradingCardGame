using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    protected ITargetCards targetCards;

    protected List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        if (targetCards == null)
            return new List<IAttackCard>();
        else
        {
            var cardsCondition = targetCards.GetTargetCards(card, battel);
            return cardsCondition.Where(x => x.BattelCard != null).ToList();
        }
    }

    private void Awake() => targetCards = GetComponent<ITargetCards>();
}
