using System.Collections.Generic;

public interface IStartingConditions
{
    bool IsConditions(IAttackCard card, List<IAttackCard> cards, IBattelBase battel);
}
