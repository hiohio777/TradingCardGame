using Zenject;

public class TrainingBattel : BaseBattel
{
    private IAlTrainingBattel alTraining;

    [Inject]
    public void Construct(IBattel battel, IUserData player, BattelFieldFactory battelFieldFactory,
        DeckBattleSelector.Factory factoriDeckBattleSelector, IAlTrainingBattel alTraining, ICardFactory<IAttackCard> cardFactory)
    {
        Initialize(battel, player, battelFieldFactory, cardFactory);
        (this.alTraining) = (alTraining);

        factoriDeckBattleSelector.Create().Build(transform, StartBattel);
        returnButton.transform.SetAsLastSibling();
    }
    public class Factory : PlaceholderFactory<TrainingBattel> { }

    private void StartBattel()
    {
        Battel.IsMasterClient = true;
        CreatPlayerPerson();
        alTraining.CreatEnemyPerson(Battel.Enemy);
        CreatBattelField();
    }

    protected override void NextTurn(object sender = null)
    {
        alTraining.NextTurn(Battel);
        base.NextTurn();
    }

    public override void FinishBattel()
    {
        battelFieldFactory.GetFinishTrainingBattel(Battel, OnLeaveBattle);
    }
}