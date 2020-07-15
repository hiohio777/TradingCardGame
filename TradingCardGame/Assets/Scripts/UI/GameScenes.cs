using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

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
    }

    private void InjectDependencies()
    {
        container.Bind(() => new List<ICollectionPanelUI> {
             UnityEngine.Object.Instantiate(Resources.Load<DecksPanel>($"CollectionScene/DecksPanel"))
                .Initialize(container.Get<IDeckFactory>(), container.Get<IFractionsData>(),
                container.Get<IUserData>(), container.Get<IEditorDeckPanel>()),
             UnityEngine.Object.Instantiate(Resources.Load<CardsCollectionPanel>($"CollectionScene/CardsCollectionPanel"))
                .Initialize(container.Get<ICardFactory<ICard>>(), container.Get<ICollectionCardsData>(),
                container.Get<IFractionsData>()),
             UnityEngine.Object.Instantiate(Resources.Load<StatisticsPanel>($"CollectionScene/StatisticsPanel")).Initialize(container.Get<Statistics>()),
         });

        // Battel UI
        container.Bind<IDeckBattleSelector>(() => UnityEngine.Object.Instantiate(Resources.Load<DeckBattleSelector>($"BattleScene/{nameof(DeckBattleSelector)}"))
            .Initialize(container.Get<IDeckFactory>(), container.Get<IFractionsData>(), container.Get<IUserData>()));

        container.Bind<BattelFieldFactory>(() => new BattelFieldFactory(container.Get<Statistics>()));

        // Игровые сцены
        container.Bind(() => UnityEngine.Object.Instantiate(Resources.Load<CollectionScene>($"CollectionScene/CollectionScene"))
            .Initialize(container.Get<IFractionsData>(), container.GetFunc<List<ICollectionPanelUI>>()));

        container.Bind(() => TrainingBattelScene.CreatPrefab(container.Get<IBattel>(), container.Get<IUserData>(),
            container.Get<IDeckBattleSelector>(), container.Get<BattelFieldFactory>(),
            container.Get<IAlTrainingBattel>()));
        container.Bind(() => CommonBattelScene.CreatPrefab(container.Get<IBattel>(), container.Get<IUserData>(),
            container.Get<BattelFieldFactory>()));
        container.Bind(() => RatingBattelScene.CreatPrefab(container.Get<IBattel>(), container.Get<IUserData>(),
            container.Get<IDeckBattleSelector>(), container.Get<BattelFieldFactory>()));
    }
}

public interface ILoaderDataGame
{
    void Load();
    void Save();
}

public class LoaderDataGame : ILoaderDataGame
{
    private ISettingsData settings;
    private IUserData userData;
    private INetworkManager networkManager;
    private ICollectionCardsData collection;

    public LoaderDataGame(ISettingsData settings, IUserData userData,
        INetworkManager networkManager, ICollectionCardsData collection)
    {
        (this.settings, this.userData, this.networkManager, this.collection)
        = (settings, userData, networkManager, collection);
        userData.SetActionSaveDecks(SaveDecks);
        Debug.Log("Game Starter!");
    }

    public void Load()
    {
        settings.Load();
        InitiateUserData(networkManager.GetUserData("login", "password"));
        Debug.Log("Data uploaded!");
    }

    public void Save()
    {
        networkManager.SendToSaveData(GetUserJ());
        Debug.Log("Data saved!");
    }

    private void SaveDecks()
    {
        networkManager.SendToSaveData(GetUserJ());
        // Временно заблокировано: networkManager.SendToSaveDecks(GetDecksJ());
    }

    private void InitiateUserData(string decksJsonString)
    {
        var data = JsonConvert.DeserializeObject<DataSerialization>(decksJsonString);

        userData.Initialize(data.user.login, data.user.gold, data.user.ram);
        foreach (var item in data)
            userData.AddNewDeck(item.name, item.fraction, item.stringCards, collection.GetCards(item.stringCards) );
    }

    private string GetUserJ()
    {
        var user = new Usererialization() { login = userData.Login, gold = userData.Gold, ram = userData.Ram, };
        var data = new DataSerialization() { user = user, decks = GetListDeck() };
        return JsonConvert.SerializeObject(data);
    }

    private string GetDecksJ()
    {
        return JsonConvert.SerializeObject(GetListDeck());
    }

    private List<DeckDataSerialize> GetListDeck()
    {
        var tempDecks = new List<DeckDataSerialize>();
        userData.Decks.ForEach(x => tempDecks.Add(new DeckDataSerialize(x.Name, x.Fraction, x.StringCards)));
        return tempDecks;
    }

    #region DataClasses
    private class Usererialization
    {
        public string login;
        public int gold, ram;
    }
    private class DataSerialization
    {
        public Usererialization user;
        public List<DeckDataSerialize> decks;

        public IEnumerator<DeckDataSerialize> GetEnumerator() => decks.GetEnumerator();
    }
    private class DeckDataSerialize
    {
        public string name;
        public string fraction;
        public List<string> stringCards;

        public DeckDataSerialize(string name, string fraction, List<string> stringCards)
        {
            this.name = name ?? throw new System.ArgumentNullException(nameof(name));
            this.fraction = fraction ?? throw new System.ArgumentNullException(nameof(fraction));
            this.stringCards = stringCards ?? throw new System.ArgumentNullException(nameof(stringCards));
        }
    }
    #endregion
}

