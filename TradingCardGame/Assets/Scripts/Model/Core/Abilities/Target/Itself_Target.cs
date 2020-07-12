using System.Collections.Generic;
using UnityEngine;

public class Itself_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel) =>
            new List<IAttackCard>() { card };
}
