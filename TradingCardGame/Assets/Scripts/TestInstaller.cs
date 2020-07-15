using Zenject;
using UnityEngine;
using System.Collections;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<INetworkManager>().To<NetworkManager>().AsSingle();
        Container.Bind<ISettingsData>().To<SettingsData>().AsSingle();
        Container.Bind<IFractionsData>().To<FractionsData>().AsSingle();
        Container.Bind<IUserData>().To<User>().AsSingle();
        Container.Bind(typeof(IStatistics), typeof(IStatisticsBattele)).To<Statistics>().AsSingle();
        Container.Bind<ILoaderDataGame>().To<LoaderDataGame>().AsSingle();
        Container.Bind<ICollectionCardsData>().To<CollectionCardsData>().AsSingle();
        Container.Bind<ISpecificityFactory>().To<SpecificityFactory>().AsSingle();
        Container.Bind<IBuffUIParametersFactory>().To<BuffUIParametersFactory>().AsSingle();

        Container.Bind<IAbilityFactory>().To<AbilityFactory>().AsSingle();
        Container.Bind<ICardFactory<ICard>>().To<CardFactory>().AsSingle();
        Container.Bind<ICardFactory<IAttackCard>>().To<BattelCardFactory>().AsSingle();
        Container.Bind<IAlTrainingBattel>().To<AlTrainingBattel>().AsSingle();

        Container.Bind(typeof(IBattel), typeof(IBattelStateData)).To<BattelData>().AsSingle();
        Container.Bind<IBattelState>().To<StartingHandState>().AsTransient();
        Container.Bind(typeof(ICardResetCounter), typeof(ICardResetCounterUI)).To<ResetCardsCounter>().AsTransient();
        Container.Bind<IBattelPersonPlayer>().To<BattelPersonPlayer>().AsTransient();
        Container.Bind<IBattelPersonEnemy>().To<BattelPersonEnemy>().AsTransient();
        Container.Bind<ICommandsExecutor<IBattelCommand>>().To<BattelCommandsExecutor>().AsTransient();

        // UI
        Container.Bind<IEditorDeckPanel>().FromComponentInNewPrefabResource($"CollectionScene/DeckEditorPanel").AsTransient();

        Container.Bind<IPanelUI>().FromComponentInNewPrefabResource($"MainScene/ArenaPanel").AsTransient();
        Container.Bind<IPanelUI>().FromComponentInNewPrefabResource($"MainScene/CompanyPanel").AsTransient();
        Container.Bind<IPanelUI>().FromComponentInNewPrefabResource($"MainScene/ShopPanel").AsTransient();
        Container.Bind<IPanelUI>().FromComponentInNewPrefabResource($"MainScene/SettingsPanel").AsTransient();

        // Игровые сцены
        Container.BindInterfacesAndSelfTo<MainScene>().FromComponentInNewPrefabResource($"MainScene/MainScene").AsSingle().NonLazy();


        Container.Bind<ApplicationGame>().FromComponentInNewPrefabResource($"ApplicationGame").AsSingle().NonLazy();
        
        //Container.Bind<>().To<>().AsSingle();
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