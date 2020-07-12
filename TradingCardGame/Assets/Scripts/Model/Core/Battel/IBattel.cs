using System;

public interface IBattelBase
{
    IBattelPerson Player { get; }
    IBattelPerson Enemy { get; }
    IBattelSpecific BattelSpecific { get; }
    ICardResetCounter CardResetCounter { get; }
    IBattelCombatData CombatData { get; }
    void OnFinishBattel(TypePersonEnum loser);
}

public interface IBattel : IBattelBase
{
    event Action NextTurn;
    event Action<BattelStateEnum> AssignBattelState;
    event Action<bool> InteractableButtonNextTurn;
    event Action<TypePersonEnum> FinishBattel;
    event Action<string> SendReportRPC;

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