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
    event Action<object> NextTurn;
    event Action<BattelStateEnum> AssignBattelState;
    event Action<bool> InteractableButtonNextTurn;
    event Action FinishBattel;
    event Action<string> SendReportRPC;
    event Action<bool> ActiveTimerBattel;

    bool IsMasterClient { get; set; }
    BattelStateEnum CurrentBattelState { get; }
    void ReportReadinessEnemy();
    void ReportReadinessPlayer();
}

public interface IBattelStateData : IBattelBase
{
    IBattelState BattelState { get; }
    void AssingNewState(IBattelState battelState);
    void OnInteractableButtonNextTurn(bool isInteractable);
    void InitialDefinitionFortune();
    void OnNextTurn();
}