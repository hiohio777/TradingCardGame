using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScenes
{
    public static GameScenes Init {
        get {
            if (init == null)
                return init = new GameScenes();
            return init;
        }
    }
    private static GameScenes init;

    private readonly DIContainer container;
    private ScenesEnum currentScene = ScenesEnum.MainScenes;

    private GameScenes()
    {
        // DI
        container = new DIContainer();
        InjectDependencies();
        LoadData();
    }

    public void StartNewScene()
    {
        switch (currentScene)
        {
            case ScenesEnum.MainScenes: container.Resolve<MainScene>(); break;
            case ScenesEnum.CollectionScene: container.Resolve<CollectionScene>(); break;
            case ScenesEnum.TrainingBattelScenes: container.Resolve<TrainingBattelScene>(); break;
            case ScenesEnum.CommonBattelScenes: container.Resolve<CommonBattelScene>(); break;
            case ScenesEnum.RatingBattelScenes: container.Resolve<RatingBattelScene>(); break;
        }
    }

    private void LoadData()
    {
        container.ResolveSinglton<ISettingsData>().Load();
        container.ResolveSinglton<User>().LoadData();
    }

    private void StartScene(ScenesEnum scene)
    {
        currentScene = scene;
        //Очистить буффер фабрик
        container.ResolveSinglton<ICardFactory<ICard>>().ClearBuffer();
        container.ResolveSinglton<ICardFactory<IAttackCard>>().ClearBuffer();
        container.ResolveSinglton<IDeckFactory>().ClearBuffer();
        container.ResolveSinglton<IBuffUIParametersFactory>().ClearBuffer();

        SceneManager.LoadScene("Scene");
    }

    private void InjectDependencies()
    {
        // модель
        container.RegisterSinglton<INetworkManager>(() => new NetworkManager());
        container.RegisterSinglton<ISettingsData>(() => new SettingsData());
        container.RegisterSinglton<Statistics>(() => new Statistics());
        container.RegisterSinglton<IFractionsData>(() => new FractionsData());
        container.RegisterSinglton<User>(() => new User(container.ResolveSinglton<INetworkManager>(),
            new UserDecks(container.ResolveSinglton<ICollectionCardsData>())));
        container.RegisterSinglton<ICollectionCardsData>(() => new CollectionCardsData(container.ResolveSinglton<IFractionsData>()));

        container.RegisterSinglton<ISpecificityFactory>(() => new SpecificityFactory());
        container.RegisterSinglton<IDeckFactory>(() => new DeckFactory()); 
        container.RegisterSinglton<IBuffUIParametersFactory>(() => new BuffUIParametersFactory());
        container.RegisterSinglton<IAbilityFactory>(() => new AbilityFactory(container.ResolveSinglton<ISpecificityFactory>()));
        container.RegisterSinglton<ICardFactory<ICard>>(() => new CardFactory(container.ResolveSinglton<ISpecificityFactory>()));
        container.RegisterSinglton<ICardFactory<IAttackCard>>(() => new BattelCardFactory(container.ResolveSinglton<ISpecificityFactory>(),
            container.ResolveSinglton<IAbilityFactory>(), container.ResolveSinglton<IBuffUIParametersFactory>()));

        container.Register<IAlTrainingBattel>(() => new AlTrainingBattel());

        container.Register<IBattel>(() => new BattelData(new BattelPersonPlayer(container.ResolveSinglton<IFractionsData>(),
            container.ResolveSinglton<ICollectionCardsData>(), container.ResolveSinglton<ICardFactory<IAttackCard>>()),
            new BattelPersonEnemy(container.ResolveSinglton<IFractionsData>(), container.ResolveSinglton<ICollectionCardsData>(),
            container.ResolveSinglton<ICardFactory<IAttackCard>>()), new StartingHandState(), new ResetCardsCounter()));

        // Отдельные элементы UI
        container.Register<IEditorDeckPanel>(() => Object.Instantiate(Resources.Load<DeckEditorPanel>($"CollectionScene/DeckEditorPanel"))
            .Initialize(container.ResolveSinglton<ICardFactory<ICard>>(), container.ResolveSinglton<ICollectionCardsData>(),
            container.ResolveSinglton<IFractionsData>(), container.ResolveSinglton<User>()));

        //Панели UI
        container.Register(() => new List<IPanelUI> {
             Object.Instantiate(Resources.Load<ArenaPanel>($"MainScene/ArenaPanel")).Initialize(StartScene, container.ResolveSinglton<User>()),
             Object.Instantiate(Resources.Load<CompanyPanel>($"MainScene/CompanyPanel")).Initialize(),
             Object.Instantiate(Resources.Load<ShopPanel>($"MainScene/ShopPanel")).Initialize(),
             Object.Instantiate(Resources.Load<SettingsPanel>($"MainScene/SettingsPanel")).Initialize(container.ResolveSinglton<ISettingsData>()),
         });

        container.Register(() => new List<ICollectionPanelUI> {
             Object.Instantiate(Resources.Load<DecksPanel>($"CollectionScene/DecksPanel"))
                .Initialize(container.ResolveSinglton<IDeckFactory>(), container.ResolveSinglton<IFractionsData>(),
                container.ResolveSinglton<User>(), container.Resolve<IEditorDeckPanel>()),
             Object.Instantiate(Resources.Load<CardsCollectionPanel>($"CollectionScene/CardsCollectionPanel"))
                .Initialize(container.ResolveSinglton<ICardFactory<ICard>>(), container.ResolveSinglton<ICollectionCardsData>(),
                container.ResolveSinglton<IFractionsData>()),
             Object.Instantiate(Resources.Load<StatisticsPanel>($"CollectionScene/StatisticsPanel")).Initialize(container.ResolveSinglton<Statistics>()),
         });

        // Battel UI
        container.Register<IDeckBattleSelector>(() => Object.Instantiate(Resources.Load<DeckBattleSelector>($"BattleScene/{nameof(DeckBattleSelector)}"))
            .Initialize(container.ResolveSinglton<IDeckFactory>(), container.ResolveSinglton<IFractionsData>(), container.ResolveSinglton<User>()));

        container.Register<BattelFieldFactory>(() => new BattelFieldFactory(container.ResolveSinglton<Statistics>()));

        // Игровые сцены
        container.Register(() => Object.Instantiate(Resources.Load<MainScene>($"MainScene/MainScene"))
            .Initialize(StartScene, container.ResolveFunc<List<IPanelUI>>()));
        container.Register(() => Object.Instantiate(Resources.Load<CollectionScene>($"CollectionScene/CollectionScene"))
            .Initialize(StartScene, container.ResolveSinglton<IFractionsData>(), container.ResolveFunc<List<ICollectionPanelUI>>()));

        container.Register(() => TrainingBattelScene.CreatPrefab(StartScene, container.Resolve<IBattel>(), container.ResolveSinglton<User>(),
            container.Resolve<IDeckBattleSelector>(), container.Resolve<BattelFieldFactory>(),
            container.Resolve<IAlTrainingBattel>()));
        container.Register(() => CommonBattelScene.CreatPrefab(StartScene, container.Resolve<IBattel>(), container.ResolveSinglton<User>(),
            container.Resolve<BattelFieldFactory>()));
        container.Register(() => RatingBattelScene.CreatPrefab(StartScene, container.Resolve<IBattel>(), container.ResolveSinglton<User>(),
            container.Resolve<IDeckBattleSelector>(), container.Resolve<BattelFieldFactory>()));
    }
}