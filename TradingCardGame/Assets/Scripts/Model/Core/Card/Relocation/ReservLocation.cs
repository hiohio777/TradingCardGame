using System;
using System.Collections.Generic;
using UnityEngine;

public class ReservLocation : ActionModuleEvent
{
    private readonly int y = 15, z = 4;

    public ReservLocation(List<IAttackCard> cards, Action actEndRelocation = null, float yPosition = -450, int offset = 140)
        : base(() => { SetSortingOrder(cards); actEndRelocation?.Invoke(); }, cards.Count) =>
        Implement(cards, yPosition, offset);

    public ReservLocation(List<IAttackCard> cards) : base(() => SetSortingOrder(cards), cards.Count) =>
        Implement(cards);

    private void Implement(List<IAttackCard> cards, float yPosition = -450, int offset = 140)
    {
        if (cards.Count % 2 > 0) ImplementOdd(cards, (cards.Count - 1) * offset / 2, yPosition, offset);
        else ImplementEven(cards, (cards.Count - 1) * offset / 2, yPosition, offset);
    }

    private void ImplementOdd(List<IAttackCard> cards, int offsetTemp, float yPosition, int offset)
    {
        int yTemp = 0;
        int temp = cards.Count / 2;
        int zTemp = temp * z;

        for (int i = 0; i < cards.Count; i++)
        {
            if (i <= temp)
            {
                if (i == temp) yTemp += y;
                else yTemp += y;
            }
            if (i > temp) yTemp -= y;

            var pos = new Vector3(offset * i - offsetTemp, yPosition + yTemp, 0);
            cards[i].SetSortingOrder(i + 1).Moving.SetPosition(pos).SetRotation(zTemp).Run(0.3f, Final);

            zTemp -= z;
        }
    }

    private void ImplementEven(List<IAttackCard> cards, int offsetTemp, float yPosition, int offset)
    {
        int yTemp = 0;
        int temp = cards.Count / 2;
        int zTemp = temp * z;

        for (int i = 0; i < cards.Count; i++)
        {
            if (i < temp) yTemp += y;
            if (i > temp) yTemp -= y;

            var pos = new Vector3(offset * i - offsetTemp, yPosition + yTemp, 0);
            cards[i].SetSortingOrder(i + 1).Moving.SetPosition(pos).SetRotation(zTemp).Run(0.3f, Final);

            if (i != temp - 1) zTemp -= z;
            else zTemp -= 2 * z;
        }
    }

    private static void SetSortingOrder(List<IAttackCard> cards)
    {
        for (int i = 0; i < cards.Count; i++)
            cards[i].SetSortingOrder(i + 1);
    }
}