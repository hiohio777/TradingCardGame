using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class RatingBattelScene : BaseBattelScene
{
    [SerializeField] private PhotonUnityNetwork photon = null;
    [SerializeField] private WaitingPanel waitingPanel = null;

    [HideInInspector] public bool isEnemyCameOut = false; // Враг вышел из боя(досрочно) Ему будет засчитано поражение(если isStartBattel = true)

    public static RatingBattelScene CreatPrefab(Action<ScenesEnum> startNewScene, IBattel battelData, IUser player,
             IDeckBattleSelector deckBattleSelector, BattelFieldFactory battelFieldFactory) =>
     (Instantiate(Resources.Load<RatingBattelScene>($"BattleScene/RatingBattelScene")).Initialize(startNewScene, battelData, player,
     battelFieldFactory) as RatingBattelScene)
     .Build(deckBattleSelector);

    private RatingBattelScene Build(IDeckBattleSelector deckBattleSelector)
    {
        deckBattleSelector.Build(transform, Connect);

        timerNextTurn = Instantiate<TimerBattel>(Resources.Load<TimerBattel>("BattleScene/TimerBattelPvP"));
        timerNextTurn.transform.SetParent(transform, false);
        Battel.ActiveTimerBattel += timerNextTurn.SetActive;
        timerNextTurn.ExecuteBlockButton += SetInteractableButtonNextTurn;
        timerNextTurn.Execute += NextTurn;

        return this;
    }

    public void ConnectedToMaster()
    {
        backButton.interactable = true;
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"ConnectedToMaster: IsMasterServer:true");
            waitingPanel.StartWaitingEnemy();
        }
        else
        {
            Battel.IsMasterClient = PhotonNetwork.IsMasterClient;
        }
    }

    public void DisconnectedBattle()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            StartCoroutine(Wait());
            return;
        }

        backButton.interactable = false;
        buttonFinishBattel.interactable = false;
        buttonNextTurn.interactable = false;
        timerNextTurn.SetActive(false);
        Debug.Log($"DisconnectedBattle: isEnemyCameOut:{isEnemyCameOut}");
        PhotonNetwork.Disconnect();
    }

    public override void FinishBattel()
    {
        battelFieldFactory.GetFinishTrainingBattel(Battel, DisconnectedBattle);
    }

    public void StartBattel()
    {
        buttonFinishBattel.onClick.RemoveAllListeners();
        buttonFinishBattel.onClick.AddListener(DisconnectedBattle);

        waitingPanel.DestroyUI();
        CreatBattelField();
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
        Battel.ActiveTimerBattel -= timerNextTurn.SetActive;
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
            }
            else
            {
                Battel.Winner = TypePersonEnum.enemy;
            }

            isStartBattel = false;
            FinishBattel();
        }
        else
        {
            Debug.Log("LeaveBattle");
            OnLeaveBattle();
        }
    }
}