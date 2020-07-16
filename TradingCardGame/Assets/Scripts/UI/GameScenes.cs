using UnityEngine;

public class GameScenes
{
    private readonly DIContainer container;

    private void InjectDependencies()
    {

        // Battel UI
        container.Bind<IDeckBattleSelector>(() => UnityEngine.Object.Instantiate(Resources.Load<DeckBattleSelector>($"BattleScene/{nameof(DeckBattleSelector)}"))
            .Initialize(container.Get<IDeckFactory>(), container.Get<IFractionsData>(), container.Get<IUserData>()));

        container.Bind<BattelFieldFactory>(() => new BattelFieldFactory(container.Get<Statistics>()));


        container.Bind(() => TrainingBattelScene.CreatPrefab(container.Get<IBattel>(), container.Get<IUserData>(),
            container.Get<IDeckBattleSelector>(), container.Get<BattelFieldFactory>(),
            container.Get<IAlTrainingBattel>()));
        container.Bind(() => CommonBattelScene.CreatPrefab(container.Get<IBattel>(), container.Get<IUserData>(),
            container.Get<BattelFieldFactory>()));
        container.Bind(() => RatingBattelScene.CreatPrefab(container.Get<IBattel>(), container.Get<IUserData>(),
            container.Get<IDeckBattleSelector>(), container.Get<BattelFieldFactory>()));
    }
}
