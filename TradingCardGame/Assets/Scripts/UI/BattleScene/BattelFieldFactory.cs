using System;
using UnityEngine;

public class BattelFieldFactory
{
    private readonly IStatisticsBattele statistics;

    public BattelFieldFactory(IStatisticsBattele statistics) =>
        (this.statistics) = (statistics);

    public IBattelDataPanel GetBattelDataPanel(IBattel battel) =>
        UnityEngine.Object.Instantiate(Resources.Load<BattelDataPanel>($"BattleScene/BattelDataPanel")).Initialize(battel);
    public IPersonsPanel GetPersonsPanel(Transform parent, IBattelPerson player, IBattelPerson enemy) =>
        UnityEngine.Object.Instantiate(Resources.Load<PersonsPanel>($"BattleScene/PersonsPanel")).Initialize(parent, player, enemy);
    public IStartingHandPanel GetStartingHandPanel(Transform parent, TimerBattel timerNextTurn, IBattelPerson person, Action accept) =>
        UnityEngine.Object.Instantiate(Resources.Load<StartingHandPanel>($"BattleScene/StartingHandPanel")).Initialize(parent, timerNextTurn, person, accept);
    public IBattleFieldCards GetBattleFieldCards() =>
        UnityEngine.Object.Instantiate(Resources.Load<BattleFieldCards>($"BattleScene/BattleFieldCards")).Initialize();
    public IFinishBattel GetFinishTrainingBattel(IBattel battel, Action continueAct) =>
        UnityEngine.Object.Instantiate(Resources.Load<FinishBattel>($"BattleScene/FinishTrainingBattel"))
        .Initialize(battel, continueAct, statistics);
}
