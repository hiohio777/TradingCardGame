using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingBattelScene : BaseBattelScene
{
    [SerializeField] private PhotonUnityNetwork photon = null;
    [SerializeField] private WaitingPanel waitingPanel = null;

    public bool isEnemyCameOut = false; // Враг вышел из боя(досрочно) Ему будет засчитано поражение

    public static RatingBattelScene CreatPrefab(Action<ScenesEnum> startNewScene, IBattel battelData, IUser player,
             IDeckBattleSelector deckBattleSelector, IBattelFieldFactory battelFieldFactory) =>
     (Instantiate(Resources.Load<RatingBattelScene>($"BattleScene/RatingBattelScene")).Initialize(startNewScene, battelData, player,
     battelFieldFactory) as RatingBattelScene)
     .Build(deckBattleSelector);

    private RatingBattelScene Build(IDeckBattleSelector deckBattleSelector)
    {
        deckBattleSelector.Build(transform, Connect);
        return this;
    }

    private void Connect()
    {
        CreatPlayerPerson();

        // Серелизация данных об игроке, для последующей отправки противнику
        Battel.Player.Report = new StartBattelDATAREPORT(player.Login, player.CurrentDeck.Fraction, player.CurrentDeck.StringCards).GetJsonString();

        SetBackScene(ScenesEnum.RatingBattelScenes);
        backButton.interactable = false;
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(PhotonNetwork.Disconnect);

        waitingPanel.StartWaitConnection();
        photon.Connect(this);
    }

    public void ConnectedToMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"IsMasterServer: true");
            Battel.IsMasterServer = true; // мастер сервер комнаты(управляет ходом фаз боя)
            backButton.interactable = true;
            waitingPanel.StartWaitingEnemy();
        }
        else
        {
            Battel.IsMasterServer = false;
            backButton.interactable = true;
        }
    }

    public void DisconnectedBattle(DisconnectCause cause)
    {
        Debug.Log("DisconnectedBattle " + PhotonNetwork.IsConnected);
        if (PhotonNetwork.IsConnected)
        {
            backButton.interactable = false;
            PhotonNetwork.Disconnect();
        }
        else
        {
            if (isEnemyCameOut) FinishBattel(TypePersonEnum.enemy);
            else StartCoroutine(Wait());
        }
    }

    public override void FinishBattel(TypePersonEnum loser)
    {
        Battel.IsMasterServer = false;
        battelFieldFactory.GetFinishTrainingBattel(Battel, loser, OnLeaveBattle);
    }

    public void LeaveBattleFinith()
    {
        Debug.Log("LeaveBattle");
        Battel.IsMasterServer = false;
        OnLeaveBattle();
    }

    public void StartBattel()
    {
        backButton.onClick.RemoveAllListeners();
        buttonFinishBattel.onClick.AddListener(PhotonNetwork.Disconnect);
        waitingPanel.DestroyUI();
        CreatBattelField();
        Debug.Log("Начать битву!");
    }

    private IEnumerator Wait()
    {
        for (int i = 0; i < 100; i++)
            yield return null;
        LeaveBattleFinith();
    }
}