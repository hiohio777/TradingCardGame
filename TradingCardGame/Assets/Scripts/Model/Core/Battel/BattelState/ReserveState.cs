using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ReserveState : IBattelState
{
    public BattelStateEnum TypeBattelState { get; } = BattelStateEnum.reserve;
    private IBattelStateData battel;
    private IBattelCard current;

    public void Run(IBattelStateData battel)
    {
        this.battel = battel;
        battel.SetInteractableButtonNextTurn(true);

        battel.Player.ReservCards.ForEach(x => x.SetClickListener(SelectReserveCard));
        battel.Player.AttackCards.ForEach(x => { if (x.BattelCard == null) x.SetClickListener(PutCardFromReserve); });
    }

    public void Request(IBattelStateData battel)
    {
        Action act = () => battel.SetBattelState(new ImplementationState());
        // Реализовать действия врага
        battel.Enemy.BringCardsToBattlefield(0.8f, act);
    }

    public void ReportReadinessPlayer(Action report)
    {
        current?.Frame(false);
        battel.SetInteractableButtonNextTurn(false);

        ClearClickListener();

        battel.Player.BringCardsToBattlefield(0.3f, report);
    }

    private void SelectReserveCard(IBattelCard battelCard)
    {
        current?.SetOldSortingOrder().Frame(false);
        if (battelCard != current)
        {
            (current = battelCard)?.SetSortingOrder(200).Frame(true);
        }
        else current = null;
    }

    private void PutCardFromReserve(IAttackCard cardAttack)
    {
        if (current == null) return;

        ClearClickListener();

        cardAttack.PutCardFromReserve(current);
        current.MoveTo(0.3f, cardAttack.DefaultPosition, FinishPutCardFromReserve);

        battel.Player.ReservCards.Remove(current);
        battel.Player.MoveToReservLocation();
    }

    private void FinishPutCardFromReserve()
    {
        current.SetSortingOrder(0).SetScale(new Vector3(1, 1, 1)).Frame(false);
        current = null;

        byte count = 0;
        battel.Player.AttackCards.ForEach(x => { if (x.BattelCard != null) count++; });
        if (count == battel.Player.AttackCards.Count)
            battel.OnNextTurn();
        else
        {
            battel.Player.ReservCards.ForEach(x => x.SetClickListener(SelectReserveCard));
            battel.Player.AttackCards.ForEach(x => x.SetClickListener(PutCardFromReserve));
        }
    }

    private void ClearClickListener()
    {
        battel.Player.ReservCards.ForEach(x => x.ClearClickListener());
        battel.Player.AttackCards.ForEach(x => x.ClearClickListener());
    }
}
