using System;

public interface IBattelBase
{
    IBattelPerson Player { get; }
    IBattelPerson Enemy { get; }
    IBattelSpecific BattelSpecific { get; }
    ICardResetCounter CardResetCounter { get; }
    IBattelCombatData CombatData { get; }
    TypePersonEnum Winner { get; set; }
    void OnFinishBattel();
}

public interface IBattel : IBattelBase
{
    event Action NextTurn;
    event Action<BattelStateEnum> AssignBattelState;
    event Action<bool> InteractableButtonNextTurn;
    event Action FinishBattel;
    event Action<string> SendReportRPC;
    event Action ActiveTimerBattel;

    bool IsMasterServer { get; set; }
    BattelStateEnum CurrentBattelState { get; }
    void ReportReadinessEnemy();
    void ReportReadinessPlayer();
}

public interface IBattelStateData : IBattelBase
{
    IBattelState BattelState { get; }
    void SetBattelState(IBattelState battelState);
    void SetInteractableButtonNextTurn(bool isInteractable);
    void InitialDefinitionFortune();
    void OnNextTurn();
}