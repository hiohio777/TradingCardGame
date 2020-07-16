using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BaseBattelScene : BaseScene
{
    public IBattel Battel { get; private set; }
    public IUserData UserData { get; private set; }

    public TimerBattel timerNextTurn = null; // Нужен для режимов где на принятие решений даётся время
    protected BattelFieldFactory battelFieldFactory;
    protected bool isStartBattel = false;

    [SerializeField] protected Button backButton = null, buttonFinishBattel = null, buttonNextTurn = null;
    private ScenesEnum carrentScene = ScenesEnum.MainScene;

    protected BaseBattelScene Initialize(IBattel battel, IUserData player,
              BattelFieldFactory battelFieldFactory)
    {
        (this.Battel, this.UserData, this.battelFieldFactory) = (battel, player, battelFieldFactory);

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
        isStartBattel = true;
        Debug.Log($"Битва началась! -> {GetType().Name}");

        backButton.gameObject.SetActive(false);
        buttonFinishBattel.gameObject.SetActive(true);
        buttonNextTurn.gameObject.SetActive(true);

        battelFieldFactory.GetBattelDataPanel(Battel);
        battelFieldFactory.GetPersonsPanel(transform, Battel.Player, Battel.Enemy);
        battelFieldFactory.GetStartingHandPanel(transform, timerNextTurn, Battel.Player, NextTurn);

        var battleFieldCards = battelFieldFactory.GetBattleFieldCards();
        Battel.Enemy.AssingCells(battleFieldCards.CellEnemy);
        Battel.Enemy.AssingCells(battleFieldCards.CellPlayer);
    }

    protected virtual void NextTurn()
    {
        SetInteractableButtonNextTurn(false);
        Battel.ReportReadinessPlayer();
    }

    public abstract void FinishBattel();
    protected virtual void OnLeaveBattle() =>
        SceneManager.LoadScene(carrentScene.ToString());

    protected void SetInteractableButtonNextTurn(bool active) =>
        buttonNextTurn.interactable = active;
    protected void SetBackScene(ScenesEnum scene) =>
        carrentScene = scene;

    private void OnDisable()
    {
        Battel.InteractableButtonNextTurn -= SetInteractableButtonNextTurn;
        Battel.NextTurn -= NextTurn;
        Battel.FinishBattel -= FinishBattel;
    }
}