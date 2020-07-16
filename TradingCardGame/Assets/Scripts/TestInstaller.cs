using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("---InstallBindings");

        Container.Bind<INetworkManager>().To<NetworkManager>().AsSingle();
        Container.Bind<ISettingsData>().To<SettingsData>().AsSingle();
        Container.Bind<IFractionsData>().To<FractionsData>().AsSingle();
        Container.Bind<IUserData>().To<User>().AsSingle();
        Container.Bind(typeof(IStatistics), typeof(IStatisticsBattele)).To<Statistics>().AsSingle();
        Container.Bind<ILoaderDataGame>().To<LoaderDataGame>().AsSingle();
        Container.Bind<ICollectionCardsData>().To<CollectionCardsData>().AsSingle();
        Container.Bind<ISFXFactory>().To<SpecificityFactory>().AsSingle();
        Container.Bind<IBuffUIParametersFactory>().To<BuffUIParametersFactory>().AsSingle();
        Container.Bind<IGameLogget>().To<CommandLogger>().AsSingle();
        Container.Bind<PanelsMenager>().AsSingle();



        Container.Bind<IAbilityFactory>().To<AbilityFactory>().AsSingle();
        Container.Bind<ICardFactory<ICard>>().To<CardFactory>().AsSingle();
        Container.Bind<ICardFactory<IAttackCard>>().To<BattelCardFactory>().AsSingle();
        Container.Bind<IDeckFactory>().To<DeckFactory>().AsSingle();
        Container.Bind<IAlTrainingBattel>().To<AlTrainingBattel>().AsSingle();

        Container.Bind(typeof(IBattel), typeof(IBattelStateData)).To<BattelData>().AsSingle();

        Container.Bind<IBattelState>().To<StartingHandState>().AsTransient();
        Container.Bind(typeof(ICardResetCounter), typeof(ICardResetCounterUI)).To<ResetCardsCounter>().AsTransient();
        Container.Bind<IBattelPersonPlayer>().To<BattelPersonPlayer>().AsTransient();
        Container.Bind<IBattelPersonEnemy>().To<BattelPersonEnemy>().AsTransient();

        // UI
        Container.Bind<IEditorDeckPanel>().FromComponentInNewPrefabResource($"MainScene/DeckEditorPanel").AsSingle();
        Container.Bind<BaseGameButton<bool>>().FromComponentInNewPrefabResource($"MainScene/ToReturnButton").AsSingle();

        Container.Bind<IPanelUI>().To<MainMenuPanel>().FromComponentInNewPrefabResource($"MainScene/MainMenuPanel").AsSingle();
        Container.Bind<IPanelUI>().To<ArenaPanel>().FromComponentInNewPrefabResource($"MainScene/ArenaPanel").AsSingle();
        Container.Bind<IPanelUI>().To<CompanyPanel>().FromComponentInNewPrefabResource($"MainScene/CompanyPanel").AsSingle();
        Container.Bind<IPanelUI>().To<ShopPanel>().FromComponentInNewPrefabResource($"MainScene/ShopPanel").AsSingle();
        Container.Bind<IPanelUI>().To<SettingsPanel>().FromComponentInNewPrefabResource($"MainScene/SettingsPanel").AsSingle();

        //Container.BindInterfacesTo<CollectionScene>().FromComponentInNewPrefabResource($"MainScene/CollectionScene").AsSingle();
        Container.Bind<ICollectionPanelUI>().To<DecksPanel>().FromComponentInNewPrefabResource($"MainScene/DecksPanel").AsSingle();
        Container.Bind<ICollectionPanelUI>().To<CardsCollectionPanel>().FromComponentInNewPrefabResource($"MainScene/CardsCollectionPanel").AsSingle();
        Container.Bind<ICollectionPanelUI>().To<StatisticsPanel>().FromComponentInNewPrefabResource($"MainScene/StatisticsPanel").AsSingle();



        Container.Bind<ApplicationGame>().FromComponentInNewPrefabResource($"ApplicationGame").AsSingle().NonLazy();


        //Container.Bind<>().To<>().AsSingle();

        //Container.Bind<>().To<>().AsSingle();
        //Container.Bind<>().To<>().AsSingle();
        //Container.Bind<>().To<>().AsSingle();
        //Container.Bind<>().To<>().AsSingle();

    }

    public class Greeter
    {
        public Greeter(string message)
        {
            Debug.Log(message);
        }
    }
}