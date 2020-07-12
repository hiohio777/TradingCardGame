using System;
using System.Collections.Generic;
using UnityEngine;

public class StartingHandLocation
{
    private readonly List<Vector2> pisition = new List<Vector2>()
        { new Vector2(-500, 60), new Vector2(-300, 88), new Vector2(-100, 100),
          new Vector2(100, 100), new Vector2(300, 88), new Vector2(500, 60) };
    private readonly List<float> rotation = new List<float>() { 10, 6, 2, -2, -6, -10 };

    public StartingHandLocation(List<IBattelCard> cards)
    {
        for (int i = 0; i < cards.Count; i++)
            cards[i].SetSortingOrder(i + 1).Moving.SetPosition(pisition[i]).SetRotation(rotation[i]).Run(0.3f);
    }
}
