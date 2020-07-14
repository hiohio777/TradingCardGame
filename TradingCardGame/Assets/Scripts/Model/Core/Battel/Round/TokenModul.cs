using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TokenModul
{
    protected readonly IBattelBase battel;
    protected readonly Action actionFinish;
    protected List<IAttackCard> cards;
    protected string startToken = string.Empty;

    public TokenModul(IBattelBase battel, Action actionFinish) =>
        (this.battel, this.actionFinish) = (battel, actionFinish);

    public void Execute()
    {
        cards = battel.GetAllAttackCards();
        if (startToken != string.Empty) Apply(startToken);
        else throw new Exception("Стартовый токен не указан!");
    }

    protected void Apply(string nameToken)
    {
        var cardsWithToken = new List<IAttackCard>();
        foreach (var item in cards)
        {
            if (item.Tokens.GetCountToken(nameToken) > 0)
                cardsWithToken.Add(item);
        }

        if (cardsWithToken.Count > 0) Implement(nameToken, cardsWithToken);
        else NextToken(nameToken);
    }

    protected abstract void NextToken(string nameToken);
    protected abstract void Implement(string nameToken, List<IAttackCard> cardsWithToken);
}
