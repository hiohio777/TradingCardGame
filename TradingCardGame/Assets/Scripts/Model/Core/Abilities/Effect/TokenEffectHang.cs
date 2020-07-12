using System.Collections.Generic;
using UnityEngine;

public class TokenEffectHang : MonoBehaviour, IEffect
{
    [SerializeField] private TokenScriptableObject token;
    [SerializeField] private byte count = 1;
    [SerializeField, Space(10)] private TypeSpecificityEnum specificityTarget = TypeSpecificityEnum.Default;

    public bool Execute(IAttackCard card, List<IAttackCard> cardsTarget, IBattelBase battel, ISpecificityFactory specificityFactory)
    {
        bool result = false;
        foreach (var item in cardsTarget)
        {
            if (item.BattelCard.Tokens.AddToken(token, count))
            {
                result = true;
                item.BattelCard.StartSpecificity(specificityTarget);
            }
        }

        return result;
    }
}
