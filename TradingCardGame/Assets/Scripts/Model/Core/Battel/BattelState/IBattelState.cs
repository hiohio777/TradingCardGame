using System;

public interface IBattelState
{
    BattelStateEnum TypeBattelState { get; }
    void Request(IBattelStateData battel);
    void ReportReadinessPlayer(Action report);
    void Run(IBattelStateData battel);
}