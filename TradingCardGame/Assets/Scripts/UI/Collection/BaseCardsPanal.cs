using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCardsPanal : MonoBehaviour
{
    protected ICardFactory<ICard> cardFactory;
    protected ICollectionCardsData collection;

    protected List<ICardData> cards;
    protected readonly List<ICard> cardsUI = new List<ICard>();
    protected Action<ICard> clickCard;

    [SerializeField] protected int countCard = 0;
    [SerializeField, Space(10)] protected Text countPagesText = null;
    [SerializeField, Space(10)] protected Button previousPage = null;
    [SerializeField] protected Button nextPage = null;

    protected int countPages, currentPage, countCollectionCard;

    public virtual void Build(ICardFactory<ICard> cardFactory, ICollectionCardsData collection, Action<ICard> clickCard)
    {
        (this.cardFactory, this.collection, this.clickCard) = (cardFactory, collection, clickCard);

        previousPage.onClick.AddListener(() => FlipPage(-1));
        nextPage.onClick.AddListener(() => FlipPage(+1));

        currentPage = 1;
    }

    public void DestroyCardUI()
    {
        cardsUI.ForEach(x => { x.SetClickListener(OnClickCard); x.DestroyUI(); });
        cardsUI.Clear();
    }

    protected virtual void OnClickCard(ICard card) => clickCard.Invoke(card);

    protected void LastPages(int count)
    {
        CheckPages(count);
        currentPage = countPages;
        DisplayPage();
    }

    protected void CarrentPages(int count)
    {
        CheckPages(count);
        DisplayPage();
    }

    protected void FirstPages(int count)
    {
        CheckPages(count);
        currentPage = 1;
        DisplayPage();
    }

    protected virtual void CreatCardUI(int i)
    {
        var card = cardFactory.GetCard(cards[i]);
        card.View.SetParent(transform).SetSortingOrder(i);
        cardsUI.Add(card);
    }

    private void DisplayPage()
    {
        DestroyCardUI();

        int first = (currentPage - 1) * countCard;
        int last = first + countCard;

        for (int i = first; i < last; i++)
        {
            if (i < countCollectionCard)
            {
                CreatCardUI(i);
            }
            else break;
        }

        countPagesText.text = $"{currentPage} / {countPages}";
    }

    private void CheckPages(int count)
    {
        countCollectionCard = count;
        countPages = count / countCard;
        countPages = (count % countCard) > 0 ? countPages + 1 : countPages;
        currentPage = currentPage > countPages ? countPages : currentPage;

        currentPage = currentPage < 1 ? 1 : currentPage;
        countPages = countPages < 1 ? 1 : countPages;
    }

    private void FlipPage(int arg)
    {
        var tempCurrentPage = currentPage + arg;
        if (tempCurrentPage > countPages || tempCurrentPage < 1)
            return;

        currentPage = tempCurrentPage;
        DisplayPage();
    }
}