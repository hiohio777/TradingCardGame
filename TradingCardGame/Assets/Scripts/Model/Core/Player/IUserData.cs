using System;
using System.Collections.Generic;

public interface IUserData
{
    string Login { get; }
    int Gold { get; }
    int Ram { get; }
    TypeBattelEnum CurrentTypeBattel { get; set; }
    void SetActionSaveDecks(Action SaveDecks);

    List<IDeckData> Decks { get; }
    IDeckData CurrentDeck { get; set; }
    List<IDeckData> GetFractionDeck(string fraction);
    IDeckData GetNewDeck(string fraction, string nameDeck = "Done");
    void AddNewDeck(string name, string fraction, List<string> stringCards, List<ICardData> cards);
    void SaveDecks();
    void Initialize(string login, int gold, int ram);
}
