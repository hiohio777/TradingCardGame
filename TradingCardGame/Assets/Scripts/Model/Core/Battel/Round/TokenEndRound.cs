using System;
using System.Collections.Generic;

public class TokenEndRound : TokenModul
{
    public TokenEndRound(IBattelBase battel, Action actionFinish) : base(battel, actionFinish)
    { startToken = "Spoilage"; }

    protected override void NextToken(string nameToken)
    {
        switch (nameToken)
        {
            case "Spoilage": actionFinish.Invoke(); break;
        }
    }

    protected override void Implement(string nameToken, List<IAttackCard> cardsWithToken)
    {
        switch (nameToken)
        {
            case "Spoilage": ApplySpoilage(cardsWithToken); break;
        }
    }

    private void ApplySpoilage(List<IAttackCard> cardsWithToken)
    {
        foreach (var item in cardsWithToken)
            item.BattelCard.Combat.TokenDamege((sbyte)(2 * item.BattelCard.Tokens.GetCountToken("Spoilage")));

        Action act = () => new DeathCards(battel, () => NextToken("Spoilage"), cards).Execute();
        new ActionSpecificitiesModule(TypeSpecificityEnum.AttackUpAll, cardsWithToken, act);
    }
}