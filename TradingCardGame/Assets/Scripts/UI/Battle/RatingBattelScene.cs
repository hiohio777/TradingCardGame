using System.Collections;
using Photon.Pun;
using UnityEngine;

public class RatingBattelScene : BaseBattel
{
    [SerializeField] private PhotonUnityNetwork photon = null;
    [SerializeField] private WaitingPanel waitingPanel = null;

    [HideInInspector] public bool isEnemyCameOut = false; // Враг вышел из боя(досрочно) Ему будет засчитано поражение(если isStartBattel = true)

    public static RatingBattelScene CreatPrefab(IBattel battelData, IUserData player, DeckBattleSelector deckBattleSelector,
        BattelFieldFactory battelFieldFactory, ICardFactory<IAttackCard> cardFactory) =>
     (Instantiate(Resources.Load<RatingBattelScene>($"BattleScene/RatingBattelScene")).Initialize(battelData, player,
     battelFieldFactory, cardFactory) as RatingBattelScene)
     .Build(deckBattleSelector);

    private RatingBattelScene Build(DeckBattleSelector deckBattleSelector)
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
        returnButton.Interactable = true;
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

    public void DisconnectedBattle(object sender = null)
    {
        if (PhotonNetwork.IsConnected == false)
        {
            StartCoroutine(Wait());
            return;
        }

        returnButton.Interactable = false;
        buttonFinishBattel.Interactable = false;
        buttonNextTurn.Interactable = false;
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
        buttonFinishBattel.ClearListener();
        buttonFinishBattel.SetListener(DisconnectedBattle);

        waitingPanel.DestroyUI();
        CreatBattelField();
    }

    private void Connect()
    {
        CreatPlayerPerson();

        //SetBackScene(PanelNameEnum.RatingBattel);
        returnButton.Interactable = false;
        returnButton.ClearListener();
        returnButton.SetListener(DisconnectedBattle);

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