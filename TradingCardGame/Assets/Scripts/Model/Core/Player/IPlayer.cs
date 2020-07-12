using System.Collections.Generic;

public interface IUser
{
    string Login { get; }
    IDeckData CurrentDeck { get; set; }
}

public interface IUserDecks
{
    List<IDeckData> Decks { get; }
    IDeckData CurrentDeck { get; set; }

    List<IDeckData> GetFractionDeck(string fraction);
    IDeckData GetNewDeck(string fraction, string nameDeck = "Done");
    void SaveDecks();
}

public interface IUserData
{
    string Login { get; }
    int Gold { get; }
    int Ram { get; }
}
