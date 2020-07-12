using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class RatingBattelScene : BaseBattelScene
{
    [SerializeField] private PhotonUnityNetwork photon = null;
    [SerializeField] private WaitingPanel waitingPanel = null;

    [HideInInspector] public bool isEnemyCameOut = false; // Враг вышел из боя(досрочно) Ему будет засчитано поражение(если isStartBattel = true)
    [HideInInspector] public bool isStartBattel = false; // Битва начата(оба игрока установили соединение)

    public static RatingBattelScene CreatPrefab(Action<ScenesEnum> startNewScene, IBattel battelData, IUser player,
             IDeckBattleSelector deckBattleSelector, BattelFieldFactory battelFieldFactory) =>
     (Instantiate(Resources.Load<RatingBattelScene>($"BattleScene/RatingBattelScene")).Initialize(startNewScene, battelData, player,
     battelFieldFactory) as RatingBattelScene)
     .Build(deckBattleSelector);

    private RatingBattelScene Build(IDeckBattleSelector deckBattleSelector)
    {
        deckBattleSelector.Build(transform, Connect);
        timerNextTurn.ExecuteBlockButton += SetInteractableButtonNextTurn;
        timerNextTurn.Execute += NextTurn;
        Battel.ActiveTimerBattel += timerNextTurn.StartTimer;

        return this;
    }

    public void ConnectedToMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"IsMasterServer: true");
            Battel.IsMasterServer = true; // мастер сервер комнаты(управляет ходом фаз боя)
            waitingPanel.StartWaitingEnemy();
        }
        else Battel.IsMasterServer = false;

        backButton.interactable = true;
    }

    public void DisconnectedBattle()
    {
        Debug.Log("DisconnectedBattle " + PhotonNetwork.IsConnected);
        if (PhotonNetwork.IsConnected)
        {
            backButton.interactable = false;
            PhotonNetwork.Disconnect();
        }
        else
        {
            timerNextTurn.HideTimer();
            buttonFinishBattel.interactable = false;
            buttonNextTurn.interactable = false;

            StartCoroutine(Wait());
        }
    }

    public override void FinishBattel()
    {
        isStartBattel = false;
        battelFieldFactory.GetFinishTrainingBattel(Battel, DisconnectedBattle);
    }

    public void LeaveBattleFinith()
    {
        Debug.Log("LeaveBattle");
        Battel.IsMasterServer = false;
        OnLeaveBattle();
    }

    public void StartBattel()
    {
        buttonFinishBattel.onClick.RemoveAllListeners();
        buttonFinishBattel.onClick.AddListener(DisconnectedBattle);

        waitingPanel.DestroyUI();
        CreatBattelField();
        isStartBattel = true;
        Debug.Log("Начать битву!");
    }

    private void Connect()
    {
        CreatPlayerPerson();

        SetBackScene(ScenesEnum.RatingBattelScenes);
        backButton.interactable = false;
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(DisconnectedBattle);

        waitingPanel.StartWaitConnection();
        photon.Connect(this);
    }

    private void OnDisable()
    {
        timerNextTurn.ExecuteBlockButton -= SetInteractableButtonNextTurn;
        timerNextTurn.Execute -= NextTurn;
        Battel.ActiveTimerBattel -= timerNextTurn.StartTimer;
    }

    private IEnumerator Wait()
    {
        for (int i = 0; i < 100; i++)
            yield return null;

        if (isStartBattel)
        {
            if (isEnemyCameOut) 
            {
                Battel.Winner = TypePersonEnum.player;
                FinishBattel();
            }
            else 
            {
                Battel.Winner = TypePersonEnum.enemy;
                FinishBattel();
            }
        }
        else LeaveBattleFinith();
    }
}