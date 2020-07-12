using System;
using System.Collections.Generic;
using UnityEngine;

public class BattelData : IBattel, IBattelStateData
{
    public event Action NextTurn;
    public event Action<BattelStateEnum> AssignBattelState;
    public event Action<bool> InteractableButtonNextTurn;
    public event Action<TypePersonEnum> FinishBattel;
    public event Action<string> SendReportRPC;

    public PhotonUnityNetwork Photon { get; set; }
    public bool IsMasterServer { get; set; }
    public IBattelPerson Player { get; private set; }
    public IBattelPerson Enemy { get; private set; }
    public IBattelSpecific BattelSpecific { get; private set; }
    public IBattelState BattelState { get; private set; }
    public BattelStateEnum CurrentBattelState { get => BattelState.TypeBattelState; }
    public ICardResetCounter CardResetCounter { get; }
    public IBattelCombatData CombatData { get; private set; }

    public BattelData(IBattelPerson player, IBattelPerson enemy, IBattelState battelState, ICardResetCounter cardResetCounter)
    {
        (Player, Enemy, BattelSpecific, BattelState, this.CardResetCounter, CombatData) =
        (player, enemy, new BattelSpecificData(), battelState, cardResetCounter, new BattelCombatData());

        BattelState.Run(this);
    }

    public void SetBattelState(IBattelState battelState)
    {
        BattelState = battelState;
        Player.Report = Enemy.Report = "Done";
        AssignBattelState?.Invoke(BattelState.TypeBattelState);
        BattelState.Run(this);
    }

    public void InitialDefinitionFortune()
    {
        if (!IsMasterServer)
            return;

        var random = new System.Random();
        if (random.Next(0, 2) == 0) Enemy.Fortune = !(Player.Fortune = true);
        else Enemy.Fortune = !(Player.Fortune = false);

        SendReportRPC?.Invoke("Fortune");
    }

    public void SetInteractableButtonNextTurn(bool isInteractable) => InteractableButtonNextTurn?.Invoke(isInteractable);
    public void ReportReadinessPlayer()
    {
        if (Player.IsReadyContinue) return;
        Player.IsReadyContinue = true;

        Action act = () =>
        {
            SendReportRPC?.Invoke(CurrentBattelState.ToString());
            ReportReadiness(Player, Enemy);
        };

        BattelState.ReportReadinessPlayer(act);
    }

    public void ReportReadinessEnemy()
    {
        if (Enemy.IsReadyContinue) return;
        Enemy.IsReadyContinue = true;

        ReportReadiness(Enemy, Player);
    }

    public void OnNextTurn() => NextTurn.Invoke();
    public void OnFinishBattel(TypePersonEnum loser) => FinishBattel.Invoke(loser);

    private void ReportReadiness(IBattelPerson person1, IBattelPerson person2)
    {
        if (person2.IsReadyContinue)
        {
            person1.IsReadyContinue = person2.IsReadyContinue = false;
            BattelState.Request(this);
        }
    }
}