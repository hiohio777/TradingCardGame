using System;
using System.Collections.Generic;

public class StartingHandState : IBattelState
{
    public BattelStateEnum TypeBattelState { get; } = BattelStateEnum.starting_hand;
    private IBattelStateData battel;

    public StartingHandState()
    {}

    public void Request(IBattelStateData battel)
    {
        battel.InitialDefinitionFortune();
        battel.AssingNewState(new ReserveState());
    }

    public void Run(IBattelStateData battel) =>
        this.battel = battel;

    public void ReportReadinessPlayer(Action report)
    {
        battel.Player.ReservCards.ReservLocation(report);
    }
}
