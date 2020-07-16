using System;
using UnityEngine;

public class BattelData : IBattel, IBattelStateData
{
    public event Action NextTurn;
    public event Action<BattelStateEnum> AssignBattelState;
    public event Action<bool> InteractableButtonNextTurn;
    public event Action FinishBattel;
    public event Action<string> SendReportRPC;
    public event Action<bool> ActiveTimerBattel;

    public bool IsMasterClient { get; set; }
    public IBattelPerson Player { get; private set; }
    public IBattelPerson Enemy { get; private set; }
    public IBattelSpecific BattelSpecific { get; private set; }
    public IBattelState BattelState { get; private set; }
    public BattelStateEnum CurrentBattelState { get => BattelState.TypeBattelState; }
    public ICardResetCounter CardResetCounter { get; }
    public IBattelCombatData CombatData { get; private set; }
    public TypePersonEnum Winner { get; set; }

    public BattelData(IBattelPersonPlayer player, IBattelPersonEnemy enemy, IBattelState initialState,
        ICardResetCounter cardResetCounter)
    {
        (Player, Enemy, BattelSpecific, this.CardResetCounter, CombatData) =
        (player, enemy, new BattelSpecificData(), cardResetCounter, new BattelCombatData());

        // Противники должны знать друг о друге
        Player.EnemyPerson = Enemy;
        Enemy.EnemyPerson = Player;

        AssingNewState(initialState);
    }

    // Assign a new state to a state machine
    public void AssingNewState(IBattelState battelState)
    {
        BattelState = battelState;
        SettttBattelState(battelState);
        BattelState.Run(this);
    }

    public void SettttBattelState(IBattelState battelState)
    {
        Player.Report = Enemy.Report = "Done";
        OnDisplayBattelState(BattelState.TypeBattelState);
    }

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

    private void ReportReadiness(IBattelPerson person1, IBattelPerson person2)
    {
        if (person2.IsReadyContinue)
        {
            OnActiveTimerBattel(false);
            person1.IsReadyContinue = person2.IsReadyContinue = false;
            BattelState.Request(this);
        }
        else
        {
            if (IsMasterClient == false
            || CurrentBattelState == BattelStateEnum.implementation
            || CurrentBattelState == BattelStateEnum.round)
                return;

            OnActiveTimerBattel(true);
        }
    }

    public void InitialDefinitionFortune()
    {
        if (!IsMasterClient)
            return;

        var random = new System.Random();
        if (random.Next(0, 2) == 0) Enemy.Fortune = !(Player.Fortune = true);
        else Enemy.Fortune = !(Player.Fortune = false);

        OnSendReportRPC("Fortune");
    }


    public void OnFinishBattel()
    {
        Debug.Log("Битва завершена!");
        FinishBattel.Invoke();
    }

    public void OnInteractableButtonNextTurn(bool isInteractable)
    {
        InteractableButtonNextTurn?.Invoke(isInteractable);
    }

    public void OnNextTurn()
    {
        NextTurn.Invoke();
    }

    public void OnDisplayBattelState(BattelStateEnum typeState)
    {
        AssignBattelState.Invoke(typeState);
    }

    public void OnSendReportRPC(string data)
    {
        SendReportRPC.Invoke(data);
    }

    public void OnActiveTimerBattel(bool active)
    {
        ActiveTimerBattel.Invoke(active);
    }
}