using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour, IDeckUI, IPointerClickHandler
{
    [SerializeField] private Image selectedFrame = null, fon = null, art = null;
    [SerializeField, Space(10)] private Text nameDeck = null;
    [SerializeField] LocalisationText fraction = null;
    [SerializeField] private StatusDeckUI StatusDeckUI;

    private Action<DeckUI> buffered;
    private Action onClick;
    private IDeck deck;

    public static DeckUI CreatPrefab() =>
       Instantiate(Resources.Load<DeckUI>($"Deck/{nameof(DeckUI)}"));

    public void Build(Transform parent, IDeck deck, Action<DeckUI> buffered)
    {
        (this.deck, this.buffered) = (deck, buffered);
        transform.SetParent(parent, false);
        transform.SetAsLastSibling();

        onClick = deck.OnClick;
        nameDeck.text = name = deck.Name;
        fraction.SetKey(deck.Fraction);

        deck.DestroyUI += OnDestroyUI;
        deck.Select += Select;

        if (deck.Status == StatusDeckEnum.Broken) StatusDeckUI.gameObject.SetActive(true);
        else StatusDeckUI.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void Select(bool isSelected) =>
              selectedFrame.gameObject.SetActive(isSelected);

    private void OnDestroyUI()
    {
        deck.DestroyUI -= OnDestroyUI;
        deck.Select -= Select;
        Select(false);
        gameObject.SetActive(false);

        name = "BufferedDeck";
        buffered.Invoke(this); // Поместить\вернуть в буфер для переиспользования
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select(true);
        onClick.Invoke();
    }
}