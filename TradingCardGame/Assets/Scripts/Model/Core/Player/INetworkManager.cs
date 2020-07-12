using System.Collections.Generic;

public interface INetworkManager
{
    UserDataForSerialization GetPlayerData();
    void SaveDecks(string decksJsonString);
}
