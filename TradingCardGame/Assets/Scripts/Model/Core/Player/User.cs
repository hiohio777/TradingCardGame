using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class User : IUser, IUserData, IUserDecks
{
    private readonly INetworkManager networkManager;
    private readonly UserDecks userDecks;
    private UserData userData;

    public User(INetworkManager networkManager, UserDecks userDecks) =>
        (this.networkManager, this.userDecks) = (networkManager, userDecks);

    // IUserData
    public string Login => userData.Login;
    public int Gold => userData.Gold;
    public int Ram => userData.Ram;

    // IDecksCollection
    public List<IDeckData> Decks { get => userDecks.Decks; set => userDecks.Decks = value; }
    public IDeckData CurrentDeck { get => userDecks.CurrentDeck; set => userDecks.CurrentDeck = value; }
    public List<IDeckData> GetFractionDeck(string fraction) => userDecks.GetFractionDeck(fraction);
    public IDeckData GetNewDeck(string fraction, string nameDeck = "Done") => userDecks.GetNewDeck(fraction, nameDeck);
    public void SaveDecks() => userDecks.SaveDecks(networkManager);

    public void LoadData()
    {
        var data = networkManager.GetPlayerData();

        userData = new UserData("User_123", 0, 0);
        userDecks.Initiate(data.decksJsonString);
    }
}

public class UserDecks
{
    public List<IDeckData> Decks { get; set; } = new List<IDeckData>();
    public IDeckData CurrentDeck { get; set; }

    private readonly ICollectionCardsData collection;

    public UserDecks(ICollectionCardsData collection) =>
        this.collection = collection;

    public void Initiate(string decksJsonString)
    {
        var serializeDecks = JsonConvert.DeserializeObject<List<SerializeDeckData>>(decksJsonString);
        foreach (var item in serializeDecks)
            Decks.Add(new DeckData(item.name, item.fraction, item.stringCards, collection.GetCards(item.stringCards)));
    }

    public void SaveDecks(INetworkManager networkManager)
    {
        var tempDecks = new List<SerializeDeckData>();
        Decks.ForEach(x => tempDecks.Add(new SerializeDeckData(x.Name, x.Fraction, x.StringCards)));
        string jsonString = JsonConvert.SerializeObject(tempDecks);
        networkManager.SaveDecks(jsonString);
    }

    public List<IDeckData> GetFractionDeck(string fraction) =>
    Decks.Where(x => x.Fraction == fraction).ToList();

    public IDeckData GetNewDeck(string fraction, string nameDeck) =>
        new DeckData(nameDeck, fraction, StatusDeckEnum.Available);
}

public class UserData
{
    public UserData(string login, int gold, int ram)
    {
        Login = login ?? throw new ArgumentNullException(nameof(login));
        Gold = gold;
        Ram = ram;
    }

    public string Login { get; private set; }
    public int Gold { get; private set; }
    public int Ram { get; private set; }

}