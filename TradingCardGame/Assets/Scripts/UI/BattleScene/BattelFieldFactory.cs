using System;
using UnityEngine;

public class BattelFieldFactory : IBattelFieldFactory
{
    private readonly IStatisticsBattele statistics;

    public BattelFieldFactory(IStatisticsBattele statistics) =>
        (this.statistics) = (statistics);

    public IBattelDataPanel GetBattelDataPanel(IBattel battel) =>
        UnityEngine.Object.Instantiate(Resources.Load<BattelDataPanel>($"BattleScene/BattelDataPanel")).Initialize(battel);
    public IPersonsPanel GetPersonsPanel(Transform parent, IBattelPerson player, IBattelPerson enemy) =>
        UnityEngine.Object.Instantiate(Resources.Load<PersonsPanel>($"BattleScene/PersonsPanel")).Initialize(parent, player, enemy);
    public IStartingHandPanel GetStartingHandPanel(Transform parent, IBattelPerson person, Action accept) =>
        UnityEngine.Object.Instantiate(Resources.Load<StartingHandPanel>($"BattleScene/StartingHandPanel")).Initialize(parent, person, accept);
    public IBattleFieldCards GetBattleFieldCards() =>
        UnityEngine.Object.Instantiate(Resources.Load<BattleFieldCards>($"BattleScene/BattleFieldCards")).Initialize();
    public IFinishBattel GetFinishTrainingBattel(IBattel battel, TypePersonEnum loser, Action continueAct) =>
        UnityEngine.Object.Instantiate(Resources.Load<FinishTrainingBattel>($"BattleScene/FinishTrainingBattel"))
        .Initialize(battel, loser, continueAct, statistics);
}
