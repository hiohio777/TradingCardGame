using System;
using System.Collections.Generic;
using System.Linq;

public class User : IUserData
{
    private UserData userData;
    private UserDecks userDecks;
    private Action SaveDecksAct;

    // IUser
    public string Login => userData.Login;
    public int Gold => userData.Gold;
    public int Ram => userData.Ram;
    public void SetActionSaveDecks(Action SaveDecksAct)
        => this.SaveDecksAct = SaveDecksAct;

    public List<IDeckData> Decks { get => userDecks.Decks; set => userDecks.Decks = value; }
    public IDeckData CurrentDeck { get => userDecks.CurrentDeck; set => userDecks.CurrentDeck = value; }
    public List<IDeckData> GetFractionDeck(string fraction) => userDecks.GetFractionDeck(fraction);
    public IDeckData GetNewDeck(string fraction, string nameDeck = "Done") => userDecks.GetNewDeck(fraction, nameDeck);
    public void AddNewDeck(string name, string fraction, List<string> stringCards, List<ICardData> cards) =>
        userDecks.AddNewDeck(name, fraction, stringCards, cards);
    public void SaveDecks() => SaveDecksAct.Invoke();

    public void Initialize(string login, int gold, int ram)
    {
        userData = new UserData(login, gold, ram);
        userDecks = new UserDecks();
    }
}

public class UserDecks
{
    public List<IDeckData> Decks { get; set; } = new List<IDeckData>();
    public IDeckData CurrentDeck { get; set; }

    public void AddNewDeck(string name, string fraction, List<string> stringCards, List<ICardData> cards) =>
        Decks.Add(new DeckData(name, fraction, stringCards, cards));

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

    public string Login { get; private set; } = string.Empty;
    public int Gold { get; private set; }
    public int Ram { get; private set; }
}


