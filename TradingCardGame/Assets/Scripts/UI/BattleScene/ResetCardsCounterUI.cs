using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetCardsCounterUI : MonoBehaviour
{
    [SerializeField] private List<Image> indicators;
    private ICardResetCounterUI cardResetCounter;

    public void Initiate(ICardResetCounterUI cardResetCounter)
    {
        this.cardResetCounter = cardResetCounter;

        cardResetCounter.Strengthen += Enlarge;
        cardResetCounter.Clear += Clear;
        cardResetCounter.ResetCards += ResetCards;
    }

    private void Enlarge(int carrentCount)
    {
        indicators[carrentCount].gameObject.SetActive(true);
    }

    private void Clear()
    {
        indicators.ForEach(x => x.gameObject.SetActive(false));
    }

    private void ResetCards()
    {

    }

    private void Awake()
    {
        Clear();
    }

    private void OnDestroy()
    {
        if (cardResetCounter == null) return;
        cardResetCounter.Strengthen -= Enlarge;
        cardResetCounter.Clear -= Clear;
        cardResetCounter.ResetCards -= ResetCards;
    }
}