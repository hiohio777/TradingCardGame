using System.Collections.Generic;

public interface ITargetCards
{
    List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel);
}
