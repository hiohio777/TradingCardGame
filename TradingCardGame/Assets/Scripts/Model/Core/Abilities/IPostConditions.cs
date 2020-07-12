using System.Collections.Generic;

public interface IPostConditions
{
    List<IAttackCard> GetTargetCards(IAttackCard card, List<IAttackCard> cards, IBattelBase battel);
}
