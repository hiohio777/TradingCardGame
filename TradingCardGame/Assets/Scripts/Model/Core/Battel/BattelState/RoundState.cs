using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class RoundState : IBattelState
{
    public BattelStateEnum TypeBattelState { get; } = BattelStateEnum.round;
    public IBattelBase Battel { get; private set; }

    private Action finishRound;

    public void Run(IBattelStateData battel)
    {
        (this.Battel, finishRound) = (battel, battel.OnNextTurn);
        battel.SetInteractableButtonNextTurn(false);

        GeneralFunctionsRound.CreatQueue(Battel).ForEach(x => x.BattelCard.Combat.RoundData = Battel.CombatData);

        var startPhase = new StartRoundAbilityPhase(Battel, ApplyCardResetCounter);
        Battel.Player.MoveToReservLocation(startPhase.Execute, -500, 90);
    }

    public void Request(IBattelStateData battel)
    {
        // Сменить удачу
        battel.Enemy.Fortune = !(battel.Player.Fortune = !battel.Player.Fortune);

        // Пропустить резерв, если все карты на поле боя живы
        if (GeneralFunctionsRound.CreatQueue(Battel).Count == 8)
            battel.SetBattelState(new ImplementationState());
        else battel.SetBattelState(new ReserveState());
    }

    public void ReportReadinessPlayer(Action report) => report.Invoke();

    public void EndRound()
    {
        GeneralFunctionsRound.CreatQueue(Battel).ForEach(x => x.BattelCard.Combat.CountRound += 1);
        Battel.Player.MoveToReservLocation(finishRound);
    }

    public void ApplyCardResetCounter()
    {
        if (GeneralFunctionsRound.CreatQueue(Battel).Count == 8)
            Battel.CardResetCounter.OnStrengthen(Battel, () => EndRound());
        else
        {
            Battel.CardResetCounter.OnClear();
            EndRound();
        }
    }
}