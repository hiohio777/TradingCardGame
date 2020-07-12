using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditorPanel : MonoBehaviour, IEditorDeckPanel
{
    private Action back;

    private IFractionsData fractions;
    private IUserDecks userDecks;
    private IDeckData editableDeck, deck;
    private bool isNewDeck = false; // true - Создание новой колоды, false - Редактирование существующей колоды

    [SerializeField] private FractionsMenu fractionMenu = null;
    [SerializeField] private Button backButton = null, selectPanelButton = null, removeDeckButton = null;
    [SerializeField] private CardsPanelEditDeckUI cardsPanel = null;
    [SerializeField] private List<CardsDeckPanelUI> cardsDeckPanels = null;
    private CardsDeckPanelUI currentCardsDeckPanels;

    public IEditorDeckPanel Initialize(ICardFactory<ICard> cardFactory, ICollectionCardsData collection,
        IFractionsData fractions, IUserDecks userDecks)
    {
        (this.fractions, this.userDecks) = (fractions, userDecks);

        fractionMenu.Initialize(fractions.Fractions);
        fractionMenu.SetListener(SelectFraction);

        backButton.onClick.AddListener(OnBack);
        selectPanelButton.onClick.AddListener(SelectPanel);
        removeDeckButton.onClick.AddListener(RemoveDeck);

        cardsPanel.Build(cardFactory, collection, AddCardWithDeck, fractions);
        cardsDeckPanels.ForEach(x => x.Build(cardFactory, RemoveCardfromDeck));

        gameObject.SetActive(false);

        return this;
    }

    public void StartEditDeck(Action back)
    {
        this.back = back;
        isNewDeck = true;

        removeDeckButton.gameObject.SetActive(false);

        editableDeck = userDecks.GetNewDeck(fractions.CurrentFraction.Name);
        StartEditDeck();
    }

    public void StartEditDeck(IDeckData deck, Action back)
    {
        (this.deck, this.back) = (deck, back);
        isNewDeck = false;

        removeDeckButton.gameObject.SetActive(true);

        editableDeck = deck.Clone();
        StartEditDeck();
    }

    private void StartEditDeck()
    {
        cardsPanel.SetDeck(editableDeck);

        cardsDeckPanels.ForEach(x => x.SetDeck(editableDeck));
        SelectPanel(TypeInitiativeEnum.veryFast);

        fractionMenu.SetActiveBattons(new List<IFraction>() { fractions.CurrentFraction, fractions.GetFraction("neutral") });
        fractionMenu.SetSelecedButton(fractions.GetFraction(editableDeck.Fraction));

        gameObject.SetActive(true);
    }

    private void Disable()
    {
        cardsPanel.DestroyCardUI();
        cardsDeckPanels.ForEach(x => x.DestroyCardUI());

        fractions.CurrentFraction = fractions.GetFraction(editableDeck.Fraction);
        gameObject.SetActive(false);
    }

    private void SelectFraction(object fraction) =>
        cardsPanel.SelectFraction(fraction as IFraction);

    private void AddCardWithDeck(ICard card)
    {
        if (editableDeck.AddCard(card.CardData) == false) return;

        SelectPanel(card.CardData.TypeInitiative);
        currentCardsDeckPanels.AddCard(card.CardData);

        cardsPanel.UpdatePanel();
    }

    private void RemoveCardfromDeck(ICard card)
    {
        editableDeck.RemoveCard(card.CardData);
        cardsPanel.UpdatePanel();
    }

    private void SelectPanel(TypeInitiativeEnum typePanel)
    {
        if (currentCardsDeckPanels != null) currentCardsDeckPanels.Disable();
        currentCardsDeckPanels = cardsDeckPanels.Where(x => x.TypePanel == typePanel).First();
        currentCardsDeckPanels.Enable();
    }

    private void SelectPanel()
    {
        if (currentCardsDeckPanels.TypePanel == TypeInitiativeEnum.verySlow)
            SelectPanel(TypeInitiativeEnum.veryFast);
        else
            SelectPanel(currentCardsDeckPanels.TypePanel + 1);
    }
    private void OnBack()
    {
        if (editableDeck.Cards.Count < CardsDeckPanelUI.maxCardsDeck)
        {
            MessagePanel.MessageWithChoice(transform, "deck_not_assembled", ExitEditor);
            return;
        }

        if (isNewDeck) ExitEditor_NewDeck();
        else ExitEditor_EdingDeck();
    }

    private void ExitEditor_NewDeck()
    {
        Action actSaveNewDeck = () => { userDecks.Decks.Add(editableDeck); userDecks.SaveDecks(); ExitEditor(); };
        MessagePanel.MessageWithChoice(transform, "save_new_deck", actSaveNewDeck, ExitEditor);
    }

    private void ExitEditor_EdingDeck()
    {
        Action act = () => { deck.PutClon(editableDeck); userDecks.SaveDecks(); ExitEditor(); };
        if (CheckDeckChanges()) act.Invoke();
        else MessagePanel.MessageWithChoice(transform, "save_changes", act, ExitEditor);
    }

    private void ExitEditor()
    {
        Disable();
        back.Invoke();
    }

    private void RemoveDeck()
    {
        Action act = () =>
        {
            userDecks.Decks.Remove(deck); 
            userDecks.SaveDecks(); 
            ExitEditor();
        };
        MessagePanel.MessageWithChoice(transform, "remove_deck", act);
    }

    private bool CheckDeckChanges()
    {
        // Проверить были ли внесены изменения в колоду
        if (deck.Name != editableDeck.Name) return false;

        foreach (var item in editableDeck.StringCards)
            if (deck.StringCards.Contains(item) == false)
                return false;

        return true;
    }
}