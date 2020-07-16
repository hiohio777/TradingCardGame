public interface INetworkManager
{
    void SendToSaveDecks(string decksJsonString);
    void SendToSaveData(string decksJsonString);
    string GetUserData(string login, string password);
}
