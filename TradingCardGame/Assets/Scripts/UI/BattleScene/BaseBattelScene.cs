using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBattelScene : BaseScene
{
    public IBattel Battel { get; private set; }
    protected IUser player;
    protected IBattelFieldFactory battelFieldFactory;

    [SerializeField] protected Button backButton = null, buttonFinishBattel = null, buttonNextTurn = null;
    private ScenesEnum carrentScene = ScenesEnum.MainScenes;

    protected BaseBattelScene Initialize(Action<ScenesEnum> startNewScene, IBattel battel, IUser player,
              IBattelFieldFactory battelFieldFactory)
    {
        (this.startNewScene, this.Battel, this.player, this.battelFieldFactory) = (startNewScene, battel, player, battelFieldFactory);

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
        Battel.Player.Creat(player.Login, player.CurrentDeck.Fraction, player.CurrentDeck.StringCards);
    }

    protected void CreatBattelField()
    {
        battelFieldFactory.GetBattelDataPanel(Battel);
        battelFieldFactory.GetPersonsPanel(transform, Battel.Player, Battel.Enemy);
        battelFieldFactory.GetStartingHandPanel(transform, Battel.Player, NextTurn);

        var battleFieldCards = battelFieldFactory.GetBattleFieldCards();
        Battel.Enemy.SetAttackCardsPosition(battleFieldCards.GetAttackingCardsEnemy);
        Battel.Player.SetAttackCardsPosition(battleFieldCards.GetAttackingCardsPlayer);

        buttonFinishBattel.gameObject.SetActive(true);
        buttonNextTurn.gameObject.SetActive(true);

        Destroy(backButton.gameObject);
        Debug.Log($"Битва началась! -> {GetType().Name}");
    }

    protected void SetBackScene(ScenesEnum scene) => carrentScene = scene;

    protected virtual void NextTurn()
    {
        SetInteractableButtonNextTurn(false);
        Battel.ReportReadinessPlayer();
    }

    public abstract void FinishBattel(TypePersonEnum loser);
    protected virtual void OnLeaveBattle()
    {
        StartNewScen(carrentScene);
    }

    private void SetInteractableButtonNextTurn(bool active)
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