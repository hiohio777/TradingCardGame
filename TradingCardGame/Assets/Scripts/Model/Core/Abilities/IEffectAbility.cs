using System.Collections.Generic;

public interface IEffect
{
    bool Execute(IAttackCard card, List<IAttackCard> cards, IBattelBase battel, ISFXFactory specificityFactory);
}
