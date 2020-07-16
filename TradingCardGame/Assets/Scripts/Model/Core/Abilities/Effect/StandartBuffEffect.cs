using System.Collections.Generic;
using UnityEngine;

public abstract class StandartBuffEffect : MonoBehaviour
{
    [SerializeField, Space(10)] private TypeSpecificityEnum specificityTarget = TypeSpecificityEnum.Default;
    private bool result;

    protected bool Execute(List<IAttackCard> cardsTarget)
    {
        result = false;
        foreach (var item in cardsTarget)
            if (Buff(item))
            {
                result = true;
                item.StartSFX(specificityTarget);
            }

        return result;
    }

    protected abstract bool Buff(IAttackCard card);
}