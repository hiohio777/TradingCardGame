using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBattelScene : BaseScene
{
    public IBattel Battel { get; private set; }
    public IUser UserData { get; private set; }
    protected BattelFieldFactory battelFieldFactory;
    public TimerBattel timerNextTurn = null; // Нужен для режимов где на принятие решений даётся время

    [SerializeField] protected Button backButton = null, buttonFinishBattel = null, buttonNextTurn = null;
    private ScenesEnum carrentScene = ScenesEnum.MainScenes;

    protected BaseBattelScene Initialize(Action<ScenesEnum> startNewScene, IBattel battel, IUser player,
              BattelFieldFactory battelFieldFactory)
    {
        (this.startNewScene, this.Battel, this.UserData, this.battelFieldFactory) = (startNewScene, battel, player, battelFieldFactory);

        backButton.onClick.AddListener(OnLeaveBattle);
        buttonFinishBattel.onClick.AddListener(OnLeaveBattle);
        buttonNextTurn.onClick.AddListener(NextTurn);

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
        battelFieldFactory.GetBattelDataPanel(Battel);
        battelFieldFactory.GetPersonsPanel(transform, Battel.Player, Battel.Enemy);
        battelFieldFactory.GetStartingHandPanel(transform, timerNextTurn, Battel.Player, NextTurn);

        var battleFieldCards = battelFieldFactory.GetBattleFieldCards();
        Battel.Enemy.SetAttackCardsPosition(battleFieldCards.GetAttackingCardsEnemy);
        Battel.Player.SetAttackCardsPosition(battleFieldCards.GetAttackingCardsPlayer);

        buttonFinishBattel.gameObject.SetActive(true);
        buttonNextTurn.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);

        Debug.Log($"Битва началась! -> {GetType().Name}");
    }

    protected void SetBackScene(ScenesEnum scene) => carrentScene = scene;

    protected virtual void NextTurn()
    {
        if (timerNextTurn != null)
            timerNextTurn.HideTimer();

        SetInteractableButtonNextTurn(false);
        Battel.ReportReadinessPlayer();
    }

    public abstract void FinishBattel();
    protected virtual void OnLeaveBattle()
    {
        StartNewScen(carrentScene);
    }

    protected void SetInteractableButtonNextTurn(bool active)
    {
        buttonNextTurn.interactable = active;
    }

    private void OnDisable()
    {
        Battel.InteractableButtonNextTurn -= SetInteractableButtonNextTurn;
        Battel.NextTurn -= NextTurn;
        Battel.FinishBattel -= FinishBattel;
    }
}