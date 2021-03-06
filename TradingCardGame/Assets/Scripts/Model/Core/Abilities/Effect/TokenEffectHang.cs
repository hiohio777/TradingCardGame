﻿using System.Collections.Generic;
using UnityEngine;

public class TokenEffectHang : MonoBehaviour, IEffect
{
    [SerializeField] private TokenScriptableObject token;
    [SerializeField] private byte count = 1;
    [SerializeField, Space(10)] private TypeSpecificityEnum specificityTarget = TypeSpecificityEnum.Default;

    public bool Execute(IAttackCard card, List<IAttackCard> cardsTarget, IBattelBase battel, ISFXFactory specificityFactory)
    {
        bool result = false;
        foreach (var item in cardsTarget)
        {
            if (item.Tokens.AddToken(token, count))
            {
                result = true;
                item.StartSFX(specificityTarget);
            }
        }

        return result;
    }
}
