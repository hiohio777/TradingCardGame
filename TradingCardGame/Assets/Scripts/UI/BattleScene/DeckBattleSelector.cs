using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBattleSelector : MonoBehaviour, IDeckBattleSelector
{
    private IDeckFactory deckFactory;
    private IFractionsData fractions;
    private IUserData userDecks;
    private List<IDeck> decks = new List<IDeck>();
    private IDeck currentDeck;

    private Action outwalk;

    [SerializeField] private Button startBattelButton = null;
    [SerializeField] private Text textInfo = null;
    [SerializeField] private Transform panelDecks = null;
    [SerializeField] private int maxDeck = 4;
    [SerializeField] private FractionsMenu fractionMenu = null;

    public DeckBattleSelector Initialize(IDeckFactory deckFactory, IFractionsData fractions, IUserData userDecks)
    {
        (this.deckFactory, this.fractions, this.userDecks) = (deckFactory, fractions, userDecks);

        startBattelButton.onClick.AddListener(OnStartBattel);

        if (this.userDecks.CurrentDeck != null)
            fractions.CurrentFraction = fractions.GetFraction(this.userDecks.CurrentDeck.Fraction);

        if (fractions.CurrentFraction.Name == "neutral")
            fractions.CurrentFraction = fractions.Fractions[0];

       // fractionMenu.Initialize(fractions.Fractions.Where(x => x.Name != "neutral").ToList()).SetListener(SelectFraction);
        fractionMenu.SetSelecedButton(fractions.CurrentFraction);

        return this;
    }

    public void Build(Transform parent, Action outwalk)
    {
        this.outwalk = outwalk;
        transform.SetParent(parent, false);
    }

    private void SelectFraction(object fraction)
    {
        fractions.CurrentFraction = fraction as IFraction;

        decks.ForEach(x => x.Destroy());
        decks = deckFactory.GetDecks(panelDecks, userDecks.GetFractionDeck(fractions.CurrentFraction.Name), OnSelectDeck, maxDeck);

        currentDeck = null;
        foreach (var item in decks)
            if (item.DeckData == userDecks.CurrentDeck)
            {
                item.OnSelect(true);
                currentDeck = item;
                break;
            }
        ActivateStartBattelButton();
    }

    private void OnStartBattel()
    {
        outwalk.Invoke();
        Destroy(gameObject);
    }

    private void OnSelectDeck(IDeck deck)
    {
        if (deck == currentDeck || deck.Status == StatusDeckEnum.Broken) return;

        currentDeck?.OnSelect(false);
        currentDeck = deck;
        userDecks.CurrentDeck = deck.DeckData;
        ActivateStartBattelButton();
    }

    private void ActivateStartBattelButton()
    {
        if (userDecks.CurrentDeck == null) SetActiveButton(false);
        else SetActiveButton(true);
    }

    private void SetActiveButton(bool active)
    {
        startBattelButton.gameObject.SetActive(active);
        textInfo.gameObject.SetActive(!active);
    }
}
