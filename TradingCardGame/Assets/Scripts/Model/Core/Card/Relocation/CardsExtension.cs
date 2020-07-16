using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        cards.ForEach(x => x.Moving.SetPosition(x.Warrior.Cell.Position));
        cards.Relocation(execute, 0.5f);
    }
    public static void StartingHandLocation(this List<IAttackCard> cards)
    {
        List<Vector2> pisition = new List<Vector2>()
        { new Vector2(-500, 60), new Vector2(-300, 88), new Vector2(-100, 100),
          new Vector2(100, 100), new Vector2(300, 88), new Vector2(500, 60) };
        List<float> rotation = new List<float>() { 10, 6, 2, -2, -6, -10 };

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].View.SetSortingOrder(i + 1);
            cards[i].Moving.SetPosition(pisition[i]).SetRotation(rotation[i]).Run(0.3f);
        }
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