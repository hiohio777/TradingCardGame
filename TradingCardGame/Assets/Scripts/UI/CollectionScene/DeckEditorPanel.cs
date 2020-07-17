using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeckEditorPanel : PanelUI, IPanelUI, IDeckEditorPanel
{
    private Action back;

    private IFractionsData fractions;
    private IUserData userDecks;
    private IDeckData editableDeck, deck;
    private ICardFactory<ICard> cardFactory;
    private ICollectionCardsData collection;
    private FractionsMenu fractionMenu;
    private ReturnButton returnButton;
    private bool isNewDeck = false; // true - Создание новой колоды, false - Редактирование существующей колоды

    [SerializeField] private Button exitButton, selectPanelButton, removeDeckButton;
    [SerializeField] private CardsPanelEditDeckUI cardsPanel;
    [SerializeField] private List<CardsDeckPanelUI> cardsDeckPanels;
    private CardsDeckPanelUI currentCardsDeckPanels;

    [Inject]
    public void Initialize(ICardFactory<ICard> cardFactory, ICollectionCardsData collection,
        IFractionsData fractions, IUserData userDecks, FractionsMenu fractionMenu, ReturnButton returnButton) =>
        (this.cardFactory, this.collection, this.fractions, this.userDecks, this.fractionMenu, this.returnButton)
        = (cardFactory, collection, fractions, userDecks, fractionMenu, returnButton);

    protected override void Initialize()
    {
        selectPanelButton.onClick.AddListener(SelectPanel);
        removeDeckButton.onClick.AddListener(RemoveDeck);
        exitButton.onClick.AddListener(OnExit);

        cardsPanel.Build(cardFactory, collection, AddCardWithDeck, fractions);
        cardsDeckPanels.ForEach(x => x.Build(cardFactory, RemoveCardfromDeck));
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
        base.Enable();
        returnButton.SetActive(false);
        cardsPanel.SetDeck(editableDeck);

        cardsDeckPanels.ForEach(x => x.SetDeck(editableDeck));
        SelectPanel(TypeInitiativeEnum.veryFast);

        fractionMenu.transform.SetParent(transform, false);
        fractionMenu.SetListener(SelectFraction);
        fractionMenu.SetActiveBattons(new List<IFraction>() { fractions.CurrentFraction, fractions.GetFraction("neutral") });
        fractionMenu.SetSelecedButton(fractions.GetFraction(editableDeck.Fraction));
    }

    private void SelectFraction(object fraction) =>
        cardsPanel.SelectFraction(fraction as IFraction);

    private void AddCardWithDeck(ICard card)
    {
        if (editableDeck.AddCard(card.CardData))
        {
            SelectPanel(card.CardData.TypeInitiative);
            currentCardsDeckPanels.AddCard(card.CardData);

            cardsPanel.UpdatePanel();
        }
    }

    private void RemoveCardfromDeck(ICard card)
    {
        editableDeck.RemoveCard(card.CardData);
        cardsPanel.UpdatePanel();
    }

    private void SelectPanel(TypeInitiativeEnum typePanel)
    {
        if (currentCardsDeckPanels != null)
        {
            currentCardsDeckPanels.Disable();
        }

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
    private void OnExit()
    {
        if (editableDeck.Cards.Count < CardsDeckPanelUI.maxCardsDeck)
        {
            MessagePanel.MessageWithChoice(transform, "deck_not_assembled", ExitEditor);
            return;
        }
        if (isNewDeck) 
        {
            Action actSaveNewDeck = () => { userDecks.Decks.Add(editableDeck); userDecks.SaveDecks(); ExitEditor(); };
            MessagePanel.MessageWithChoice(transform, "save_new_deck", actSaveNewDeck, ExitEditor);
        }
        else 
        {
            Action act = () => { deck.PutClon(editableDeck); userDecks.SaveDecks(); ExitEditor(); };
            if (CheckDeckChanges()) act.Invoke();
            else MessagePanel.MessageWithChoice(transform, "save_changes", act, ExitEditor);
        }
    }

    private void ExitEditor()
    {
        cardsPanel.DestroyCardUI();
        cardsDeckPanels.ForEach(x => x.DestroyCardUI());
        fractions.CurrentFraction = fractions.GetFraction(editableDeck.Fraction);

        base.Disable();
        cardFactory.ClearBuffer();
        returnButton.SetActive(true);

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