using System.Collections.Generic;

public class ConditionsAbility : BaseAbility
{
    protected List<IStartingConditions> startingConditions = new List<IStartingConditions>();

    public bool IsConditions(IAttackCard card, IBattelBase battel)
    {
        var cards = GetTargetCards(card, battel);
        foreach (var item in startingConditions)
            if (item.IsConditions(card, cards, battel) == false)
                return false;

        return true;
    }

    private void Start() =>
        GetComponents(startingConditions);
}
