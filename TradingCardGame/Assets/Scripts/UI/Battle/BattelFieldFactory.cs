using System;
using System.Collections.Generic;
using UnityEngine;

public class BattelFieldFactory
{
    private readonly IStatisticsBattele statistics;
    private readonly List<GameObject> buff = new List<GameObject>();

    public BattelFieldFactory(IStatisticsBattele statistics) =>
        (this.statistics) = (statistics);

    public void Clear()
    {
        buff.ForEach(x => UnityEngine.Object.Destroy(x));
        buff.Clear();
    }

    public IBattelDataPanel GetBattelDataPanel(IBattel battel)
    {
        var prefab = UnityEngine.Object.Instantiate(Resources.Load<BattelDataPanel>($"UI/BattleUI/BattelDataPanel")).Initialize(battel);
        buff.Add(prefab.gameObject);
        return prefab;
    }

    public IPersonsPanel GetPersonsPanel(Transform parent, IBattelPerson player, IBattelPerson enemy) =>
        UnityEngine.Object.Instantiate(Resources.Load<PersonsPanel>($"UI/BattleUI/PersonsPanel")).Initialize(parent, player, enemy);
    public IStartingHandPanel GetStartingHandPanel(Transform parent, TimerBattel timerNextTurn, IBattelPerson person, Action<object> accept) =>
        UnityEngine.Object.Instantiate(Resources.Load<StartingHandPanel>($"UI/BattleUI/StartingHandPanel")).Initialize(parent, timerNextTurn, person, accept);
    public BattleFieldCards GetBattleFieldCards()
    {
        var prefab = UnityEngine.Object.Instantiate(Resources.Load<BattleFieldCards>($"UI/BattleUI/BattleFieldCards"));
        buff.Add(prefab.gameObject);
        return prefab;
    }
    public IFinishBattel GetFinishTrainingBattel(IBattel battel, Action<object> continueAct)
    {
        var prefab = UnityEngine.Object.Instantiate(Resources.Load<FinishBattel>($"UI/BattleUI/FinishTrainingBattel"))
                                                   .Initialize(battel, continueAct, statistics);
        buff.Add(prefab.gameObject);
        return prefab;
    }
}
