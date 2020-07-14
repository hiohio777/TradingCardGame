using System;
using System.Collections.Generic;
using UnityEditor;

public static class CardsExtension
{
    private static Action actNext;
    private static int count = 0;

    public static void Relocation(this List<IAttackCard> cards, Action actNext, float time) =>
        Relocation(cards, actNext, time, 0, 0);
    public static void Relocation(this List<IAttackCard> cards, Action actNext, float time, float waitTime = 0, float waitAfterTime = 0)
    {
        (CardsExtension.actNext, CardsExtension.count) = (actNext, cards.Count);
        cards.ForEach(x => x.Moving.SetWaitTime(waitTime, waitAfterTime).Run(time, Final));
    }
    public static void ReturnCardToPlace(this List<IAttackCard> cards, Action execute)
    {
        cards.ForEach(x => x.Moving.SetPosition(x.DefaultPosition));
        cards.Relocation(execute, 0.5f);
    }
    public static void ReservLocation(this List<IAttackCard> cards, Action actEndRelocation = null, float yPosition = -450, int offset = 140) =>
        new ReservLocation(cards, actEndRelocation, yPosition, offset);

    private static void Final()
    {
        if (--count == 0)
        {
            actNext?.Invoke();
            actNext = null;
        }
    }
}