using System.Collections.Generic;

public class EffectAbility : BaseAbility
{
    protected List<IPostConditions> postConditions = new List<IPostConditions>();
    protected List<IEffect> effectAbility = new List<IEffect>();

    public bool IsResult(IAttackCard card, IBattelBase battel, ISpecificityFactory specificityFactory)
    {
        var cards = GetTargetCards(card, battel);
        foreach (var item in postConditions)
            cards = item.GetTargetCards(card, cards, battel);

        bool result = false;
        foreach (var item in effectAbility)
        {
            if (item.Execute(card, cards, battel, specificityFactory))
                result = true;
        }

        return result;
    }

    private void Start()
    {
        GetComponents(postConditions);
        GetComponents(effectAbility);
    }
}
