using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Панель с картами колоды(карты показываются в зависимости от их инициативы(сгруппированы на 4-ч страничках))
/// </summary>
public class CardsDeckPanelUI : MonoBehaviour
{
    public TypeInitiativeEnum TypePanel { get => typePanel; }

    [SerializeField] private TypeInitiativeEnum typePanel; //Пустое место для карты в колоде
    [SerializeField] private Text textInfo = null, textNoCards = null, textCountCards = null;
    public const int maxCountCards = 6, maxCardsDeck = 24;

    private ICardFactory<ICard> cardFactory;
    private Action<ICard> clickCard;

    private IDeckData deck;
    private readonly List<ICard> cards = new List<ICard>();

    public void Build(ICardFactory<ICard> cardFactory, Action<ICard> clickCard)
    {
        (this.cardFactory, this.clickCard) = (cardFactory, clickCard);

        textNoCards.text = "Вам нужно набрать 6 карт";
        Disable();
    }

    public void SetDeck(IDeckData deck)
    {
        this.deck = deck;
        var tempCards = deck.Cards.Where(x => x.TypeInitiative == typePanel).ToList();
        tempCards.ForEach(x => CreatCard(x));
        SetAllText();
    }

    public void DestroyCardUI()
    {
        cards.ForEach(x => { x.SetClickListener(RemoveCard); x.Destroy(); });
        cards.Clear();
    }

    public void Enable()
    {
        textInfo.fontSize = 40;
        textInfo.fontStyle = FontStyle.BoldAndItalic;

        SetTextInfo();

        gameObject.SetActive(true);
    }
    public void Disable()
    {
        textInfo.fontSize = 30;
        textInfo.fontStyle = FontStyle.Normal;

        gameObject.SetActive(false);
    }

    public void AddCard(ICardData newCards)
    {
        CreatCard(newCards);
        SetAllText();
    }

    private void RemoveCard(ICard card)
    {
        clickCard.Invoke(card);

        cards.Remove(card);
        card.Destroy();
        SetAllText();
    }

    private void CreatCard(ICardData newCards)
    {
        var card = cardFactory.GetCard(newCards);
        card.View.SetParent(transform).SetSortingOrder(10);
        card.SetClickListener(RemoveCard);
        cards.Add(card);
    }

    private void SetAllText()
    {
        SetTextInfo();
        SetTextCountCards();
    }

    private void SetTextInfo()
    {
        if (cards.Count < maxCountCards) textInfo.color = Color.red;
        else textInfo.color = Color.green;

        textInfo.text = $"{typePanel}: {cards.Count} / {maxCountCards}";

        if (cards.Count == 0) textNoCards.gameObject.SetActive(true);
        else textNoCards.gameObject.SetActive(false);
    }

    private void SetTextCountCards()
    {
        if (deck.Cards.Count < maxCardsDeck)
        {
            textCountCards.color = Color.red;
            textCountCards.text = $"{deck.Cards.Count} / {maxCardsDeck}";
        }
        else
        {
            textCountCards.color = Color.green;
            textCountCards.text = $"{maxCardsDeck}";
        }
    }
}