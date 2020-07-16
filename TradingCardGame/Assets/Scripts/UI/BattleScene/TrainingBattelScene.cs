using UnityEngine;

public class TrainingBattelScene : BaseBattelScene
{
    private IAlTrainingBattel alTraining;

    public static TrainingBattelScene CreatPrefab(IBattel battelData, IUserData player,
             IDeckBattleSelector deckBattleSelector, BattelFieldFactory battelFieldFactory, IAlTrainingBattel alTraining) =>
    (Instantiate(Resources.Load<TrainingBattelScene>($"BattleScene/TrainingBattelScene")).Initialize(battelData, player,
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
        Battel.IsMasterClient = true;
        CreatPlayerPerson();
        alTraining.CreatEnemyPerson(Battel.Enemy);
        SetBackScene(PanelNameEnum.TrainingBattelScenes);
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
