using System;
using System.Collections.Generic;

public class TokenStartRound : TokenModul
{
    public TokenStartRound(IBattelBase battel, Action actionFinish) : base(battel, actionFinish)
    { startToken = ""; }

    protected override void NextToken(string nameToken)
    {
        switch (nameToken)
        {
            case "": actionFinish.Invoke(); break;
        }
    }

    protected override void Implement(string nameToken, List<IAttackCard> cardsWithToken)
    {
        switch (nameToken)
        {
            case "": NextToken(""); break;
        }
    }
}


