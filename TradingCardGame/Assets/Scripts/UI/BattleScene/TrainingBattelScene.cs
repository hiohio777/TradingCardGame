using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TrainingBattelScene : BaseBattelScene
{
    private IAlTrainingBattel alTraining;

    public static TrainingBattelScene CreatPrefab(Action<ScenesEnum> startNewScene, IBattel battelData, IUser player,
             IDeckBattleSelector deckBattleSelector, BattelFieldFactory battelFieldFactory, IAlTrainingBattel alTraining) =>
    (Instantiate(Resources.Load<TrainingBattelScene>($"BattleScene/TrainingBattelScene")).Initialize(startNewScene, battelData, player,
     battelFieldFactory) as TrainingBattelScene)
    .Build(deckBattleSelector, alTraining);

    private TrainingBattelScene Build(IDeckBattleSelector deckBattleSelector, IAlTrainingBattel alTraining)
    {
        deckBattleSelector.Build(transform, StartBattel);
        this.alTraining = alTraining;
        return this;
    }

    private void StartBattel()
    {
        Battel.IsMasterServer = true;
        CreatPlayerPerson();
        alTraining.CreatEnemyPerson(Battel.Enemy);
        SetBackScene(ScenesEnum.TrainingBattelScenes);
        CreatBattelField();
    }

    protected override void NextTurn()
    {
        alTraining.NextTurn(Battel);
        base.NextTurn();
    }

    public override void FinishBattel()
    {
        battelFieldFactory.GetFinishTrainingBattel(Battel, OnLeaveBattle);
    }
}
