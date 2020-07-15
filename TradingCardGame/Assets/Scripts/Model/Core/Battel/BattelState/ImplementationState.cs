using System;

public class ImplementationState : IBattelState
{
    public void Run(IBattelStateData battel)
    {
        battel.OnInteractableButtonNextTurn(false);

        new ImplementationAbility(battel, () => battel.OnNextTurn() ).Execute();
    }

    public void Request(IBattelStateData battel)
    {
        battel.AssingNewState(new TacticsState());
    }

    public void ReportReadinessPlayer(Action report) => report.Invoke();
}

public abstract class BattelStateMachine
{
    public void ReportReadiness()
    {

    }

    public void Request(IBattelStateData battel)
    {
       
    }

    public void ReportReadinessPlayer(Action report)
    {

    }
}