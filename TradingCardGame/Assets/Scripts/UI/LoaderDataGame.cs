using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LoaderDataGame : ILoaderDataGame
{
    private IUserData userData;
    private INetworkManager networkManager;
    private ICollectionCardsData collection;
    private static bool isLosd = false;

    public LoaderDataGame(IUserData userData, INetworkManager networkManager,
        ICollectionCardsData collection)
    {
        (this.userData, this.networkManager, this.collection)
        = (userData, networkManager, collection);
        userData.SetActionSaveDecks(SaveDecks);
        Debug.Log("Game Starter!");
    }

    public void Load(ApplicationGame application)
    {
        if (isLosd) return;
        InitiateUserData(networkManager.GetUserData("login", "password"));
        Debug.Log("Data uploaded!");
        isLosd = true;
        application.StartGame();
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
            userData.AddNewDeck(item.name, item.fraction, item.stringCards, collection.GetCards(item.stringCards));
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

