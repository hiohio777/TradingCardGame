using System;

public class ReserveState : IBattelState
{
    public BattelStateEnum TypeBattelState { get; } = BattelStateEnum.reserve;
    private IBattelStateData battel;
    private IAttackCard current;

    public void Run(IBattelStateData battel)
    {
        this.battel = battel;
        battel.OnInteractableButtonNextTurn(true);

        battel.Player.ReservCards.ForEach(x => x.SetClickListener(SelectReserveCard));
        battel.Player.Cell.ForEach(x => x.SetClickListener(PutCardFromReserve));
    }

    public void Request(IBattelStateData battel)
    {
        Action act = () => battel.AssingNewState(new ImplementationState());
        // Реализовать действия врага
        battel.Enemy.BringCardsToBattlefield(0.8f, act);
    }

    public void ReportReadinessPlayer(Action report)
    {
        current?.View.Frame(false);
        battel.OnInteractableButtonNextTurn(false);

        ClearClickListener();

        battel.Player.BringCardsToBattlefield(0.3f, report);
    }

    private void SelectReserveCard(IAttackCard battelCard)
    {
        current?.View.SetOldSortingOrder().Frame(false);
        if (battelCard != current)
        {
            (current = battelCard)?.View.SetSortingOrder(200).Frame(true);
        }
        else current = null;
    }

    private void PutCardFromReserve(ICellBattel cell)
    {
        if (current == null) return;

        ClearClickListener();
        battel.Player.PlaceAttackCell(current, cell);
        battel.Player.ReservCards.ReservLocation();

        current.PlaceAttackCell(cell, FinishPutCardFromReserve);
    }

    private void FinishPutCardFromReserve()
    {
        current = null;

        if (battel.Player.AttackCards.Count >= 4)
            battel.OnNextTurn();
        else
        {
            battel.Player.ReservCards.ForEach(x => x.SetClickListener(SelectReserveCard));
            battel.Player.Cell.ForEach(x => x.SetClickListener(PutCardFromReserve));
        }
    }

    private void ClearClickListener()
    {
        battel.Player.ReservCards.ForEach(x => x.ClearClickListener());
        battel.Player.Cell.ForEach(x => x.ClearClickListener());
    }
}
