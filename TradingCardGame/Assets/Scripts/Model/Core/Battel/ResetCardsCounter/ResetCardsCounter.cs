using System;
using UnityEngine;

public class ResetCardsCounter : ICardResetCounter, ICardResetCounterUI
{
    public event Action<int> Strengthen;
    public event Action Clear;
    public event Action ResetCards;

    private int carrentCount;
    private readonly int max = 3;
    private Vector3 resetDirection = new Vector3(-1000, -500);

    public void OnStrengthen(IBattelBase battel, Action finish)
    {
        Strengthen.Invoke(carrentCount);
        carrentCount++;

        if (carrentCount == max)
        {
            ResetCards.Invoke();
            DiscardAll(battel, finish);
        }
        else finish.Invoke();
    }

    public void OnClear()
    {
        Clear.Invoke();
        carrentCount = 0;
    }

    private void DiscardAll(IBattelBase battel, Action finish)
    {
        battel.Player.AttackCards.ForEach(x => x.BattelCard.Moving.SetPosition(resetDirection));
        battel.Enemy.AttackCards.ForEach(x => x.BattelCard.Moving.SetPosition(resetDirection));

        new CardsRelocation(GeneralFunctionsRound.CreatQueue(battel), () => ReturnCardsToDecks(battel, finish), 0.5f, 0.5f, 0.5f);
    }

    public void ReturnCardsToDecks(IBattelBase battel, Action finish)
    {
        battel.Player.ReturnCardsToDeck();
        battel.Enemy.ReturnCardsToDeck();

        OnClear();
        finish.Invoke();
    }
}
