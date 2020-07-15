using System;

public class RoundState : IBattelState
{
    public BattelStateEnum TypeBattelState { get; } = BattelStateEnum.round;
    public IBattelBase Battel { get; private set; }

    private Action finishRound;

    public void Run(IBattelStateData battel)
    {
        (this.Battel, finishRound) = (battel, battel.OnNextTurn);
        battel.OnInteractableButtonNextTurn(false);

        Battel.GetAllAttackCards().ForEach(x => x.Combat.RoundData = Battel.CombatData);

        var startPhase = new StartRoundAbilityPhase(Battel, ApplyCardResetCounter);
        Battel.Player.ReservCards.ReservLocation(startPhase.Execute, -500, 90);
    }

    public void Request(IBattelStateData battel)
    {
        // Сменить удачу
        battel.Enemy.Fortune = !(battel.Player.Fortune = !battel.Player.Fortune);

        // Пропустить резерв, если все карты на поле боя живы
        if (battel.GetAllAttackCards().Count == 8)
            battel.AssingNewState(new ImplementationState());
        else battel.AssingNewState(new ReserveState());
    }

    public void ReportReadinessPlayer(Action report) => report.Invoke();

    public void EndRound()
    {
        Battel.GetAllAttackCards().ForEach(x => x.Combat.CountRound += 1);
        Battel.Player.ReservCards.ReservLocation(finishRound);
    }

    public void ApplyCardResetCounter()
    {
        if (Battel.GetAllAttackCards().Count == 8)
            Battel.CardResetCounter.OnStrengthen(Battel, () => EndRound());
        else
        {
            Battel.CardResetCounter.OnClear();
            EndRound();
        }
    }
}