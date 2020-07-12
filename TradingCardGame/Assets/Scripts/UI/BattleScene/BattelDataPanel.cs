using System;
using UnityEngine;
using UnityEngine.UI;

public class BattelDataPanel : MonoBehaviour, IBattelDataPanel
{
    private IBattel battel;
    [SerializeField] private Text countRound = null;
    [SerializeField] private LocalisationText currentBattelState = null;
    [SerializeField] private ResetCardsCounterUI resetCardsCounterUI = null;

    public BattelDataPanel Initialize(IBattel battel)
    {
        this.battel = battel;
        resetCardsCounterUI.Initiate(battel.CardResetCounter as ICardResetCounterUI);

        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "MainUI";

        countRound.text = "0";

        battel.AssignBattelState += AssignBattelState;
        battel.BattelSpecific.SetRounds += SetCountRound;
        battel.BattelSpecific.SetCardResetCounter += SetCardResetCounter;

        AssignBattelState(BattelStateEnum.starting_hand);

        return this;
    }

    public void AssignBattelState(BattelStateEnum battelState)
    {
        switch (battelState)
        {
            case BattelStateEnum.starting_hand: currentBattelState.Color = Color.blue; break;
            case BattelStateEnum.reserve: currentBattelState.Color = Color.green; break;
            case BattelStateEnum.tactics: currentBattelState.Color = Color.yellow; break;
            case BattelStateEnum.round: currentBattelState.Color = Color.red; break;
        }

        currentBattelState.SetKey(battelState.ToString());
    }

    private void SetCountRound(byte count)
    {
        countRound.text = count.ToString();
    }

    private void SetCardResetCounter(byte count)
    {
        Debug.Log("Ещё не реализовано!");
    }

    private void OnDestroy()
    {
        if (battel.BattelSpecific == null) return;

        battel.BattelSpecific.SetRounds -= SetCountRound;
        battel.BattelSpecific.SetCardResetCounter -= SetCardResetCounter;
    }
}