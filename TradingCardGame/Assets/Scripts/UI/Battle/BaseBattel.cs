using System;
using UnityEngine;

public abstract class BaseBattel : MonoBehaviour
{
    public event Action ReturnBack;
    public IBattel Battel { get; private set; }
    public IUserData UserData { get; private set; }

    public TimerBattel timerNextTurn = null; // Нужен для режимов где на принятие решений даётся время
    protected BattelFieldFactory battelFieldFactory;
    protected ICardFactory<IAttackCard> cardFactory;
    protected bool isStartBattel = false;

    [SerializeField] protected GameButton returnButton;
    [SerializeField] protected GameButton buttonFinishBattel = null, buttonNextTurn = null;

    protected BaseBattel Initialize(IBattel battel, IUserData player, 
        BattelFieldFactory battelFieldFactory, ICardFactory<IAttackCard> cardFactory)
    {
        (this.Battel, this.UserData, this.battelFieldFactory, this.cardFactory)
        = (battel, player, battelFieldFactory, cardFactory);

        returnButton.SetListener(OnLeaveBattle);
        buttonFinishBattel.SetListener(OnLeaveBattle); 
        buttonNextTurn.SetListener(NextTurn);

        buttonFinishBattel.gameObject.SetActive(false);
        buttonNextTurn.gameObject.SetActive(false);

        battel.InteractableButtonNextTurn += SetInteractableButtonNextTurn;
        battel.NextTurn += NextTurn;
        battel.FinishBattel += FinishBattel;

        return this;
    }

    protected void CreatPlayerPerson()
    {
        Battel.Player.Creat(UserData.Login, UserData.CurrentDeck.Fraction, UserData.CurrentDeck.StringCards);
    }

    protected void CreatBattelField()
    {
        isStartBattel = true;
        Debug.Log($"Битва началась! -> {GetType().Name}");

        returnButton.SetActive(false);
        buttonFinishBattel.gameObject.SetActive(true);
        buttonNextTurn.gameObject.SetActive(true);

        battelFieldFactory.GetBattelDataPanel(Battel);
        battelFieldFactory.GetPersonsPanel(transform, Battel.Player, Battel.Enemy);
        battelFieldFactory.GetStartingHandPanel(transform, timerNextTurn, Battel.Player, NextTurn);

        var battleFieldCards = battelFieldFactory.GetBattleFieldCards();
        Battel.Enemy.AssingCells(battleFieldCards.CellEnemy);
        Battel.Player.AssingCells(battleFieldCards.CellPlayer);
    }

    protected virtual void NextTurn(object sender = null)
    {
        SetInteractableButtonNextTurn(false);
        Battel.ReportReadinessPlayer();
    }

    public abstract void FinishBattel();

    protected virtual void OnLeaveBattle(object sender = null)
    {
        battelFieldFactory.Clear();
        cardFactory.ClearBuffer();
        Destroy(gameObject);

        ReturnBack?.Invoke();
    }

    protected void SetInteractableButtonNextTurn(bool active) =>
        buttonNextTurn.Interactable = active;

    private void OnDisable()
    {
        Battel.InteractableButtonNextTurn -= SetInteractableButtonNextTurn;
        Battel.NextTurn -= NextTurn;
        Battel.FinishBattel -= FinishBattel;
    }

    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "Default";
    }
}