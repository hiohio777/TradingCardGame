using System;

public interface IBattelState
{
    BattelStateEnum TypeBattelState { get; }

    // Request a new state machine state
    void ReportReadinessPlayer(Action report);
    void Request(IBattelStateData battel);
    void Run(IBattelStateData battel);
}