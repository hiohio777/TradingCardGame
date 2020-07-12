using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonUnityNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameVersion = "1";
    private RatingBattelScene battelScene;
    private new PhotonView photonView;

    private bool isConnecting;
    private readonly byte maxPlayers = 2;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void Connect(RatingBattelScene battelScene)
    {
        this.battelScene = battelScene;

        isConnecting = true;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
            PhotonNetwork.NickName = battelScene.Battel.Player.Name;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        Debug.Log($"DisconnectCause: {cause}");
        if(battelScene != null) battelScene.DisconnectedBattle();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"{returnCode}: {message}");

        battelScene.DisconnectedBattle();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"{returnCode}: {message}");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayers });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"CreatedRoom: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"JoinedRoom: {PhotonNetwork.CurrentRoom.Name}");
        battelScene.ConnectedToMaster();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"PlayerEntered: {newPlayer.UserId}");
        if (PhotonNetwork.IsMasterClient)
        {
            var view = PhotonNetwork.Instantiate("BattleScene/PhotonView", new Vector3(0, 0, 0), Quaternion.identity);
            photonView = view.GetPhotonView();
        }

        if (PhotonNetwork.CurrentRoom == null)
            PhotonNetwork.Disconnect();

        // Закрыть комнату и сделать невидимой
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"PlayerLeft: {otherPlayer.UserId}");
        battelScene.isEnemyCameOut = true;
        PhotonNetwork.Disconnect();
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        Debug.Log(errorInfo);
    }
}
