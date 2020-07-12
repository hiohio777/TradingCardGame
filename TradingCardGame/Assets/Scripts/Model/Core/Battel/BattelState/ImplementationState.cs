using System;

public class ImplementationState : IBattelState
{
    public BattelStateEnum TypeBattelState { get; } = BattelStateEnum.implementation;
    private IBattelStateData battel;

    public void Run(IBattelStateData battel)
    {
        this.battel = battel;
        battel.SetInteractableButtonNextTurn(false);

        new ImplementationAbility(battel, EndImplementation).Execute();
    }

    public void Request(IBattelStateData battel)
    {
        battel.SetBattelState(new TacticsState());
    }

    public void EndImplementation()
    {
        battel.OnNextTurn();
    }

    public void ReportReadinessPlayer(Action report) => report.Invoke();
}
