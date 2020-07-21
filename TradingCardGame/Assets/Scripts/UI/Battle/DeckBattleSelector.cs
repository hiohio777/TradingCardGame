using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeckBattleSelector : MonoBehaviour
{
    private IDeckFactory deckFactory;
    private IFractionsData fractions;
    private IUserData userDecks;
    private List<IDeck> decks = new List<IDeck>();
    private IDeck currentDeck;
    private FractionsMenu fractionMenu;

    private Action outwalk;

    [SerializeField] private Button startBattelButton = null;
    [SerializeField] private Text textInfo = null;
    [SerializeField] private Transform panelDecks = null;
    [SerializeField] private int maxDeck = 4;

    [Inject]
    public void Construct(IDeckFactory deckFactory, IFractionsData fractions,
        IUserData userDecks, FractionsMenu fractionMenu)
    {
        (this.deckFactory, this.fractions, this.userDecks, this.fractionMenu) 
        = (deckFactory, fractions, userDecks, fractionMenu);

        startBattelButton.onClick.AddListener(OnStartBattel);

        if (this.userDecks.CurrentDeck != null)
            fractions.CurrentFraction = fractions.GetFraction(this.userDecks.CurrentDeck.Fraction);

        if (fractions.CurrentFraction.Name == "neutral")
            fractions.CurrentFraction = fractions.Fractions[0];

        fractionMenu.transform.SetParent(transform, false);
        fractionMenu.SetActiveBattons(fractions.Fractions.Where(x => x.Name != "neutral").ToList()).SetListener(SelectFraction);
        fractionMenu.SetSelecedButton(fractions.CurrentFraction);
    }
    public class Factory : PlaceholderFactory<DeckBattleSelector> { }

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

    private void OnDestroy()
    {
        deckFactory.ClearBuffer();
        fractionMenu.transform.SetParent(null, false);
    }
}
