using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    protected ITargetCards targetCards;

    protected List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        if (targetCards == null)
        {
            return new List<IAttackCard>();
        }
        else
        {
            return targetCards.GetTargetCards(card, battel);
        }
    }

    private void Awake() => targetCards = GetComponent<ITargetCards>();
}
