using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckFactory : IDeckFactory
{ 
    private readonly Stack<DeckUI> buffer = new Stack<DeckUI>();

    public void ClearBuffer() => buffer.Clear();

    public List<IDeck> GetDecks(Transform parent, List<IDeckData> decksData,
                Action<IDeck> onClick = null, int maxDeck = -1)
    {
        var decks = new List<IDeck>();
        foreach (var item in decksData)
        {
            decks.Add(BuildDeckUI(parent, new Deck(item, onClick)));
            if (decks.Count == maxDeck) break;
        }

        return decks;
    }

    public IDeck GetDeck(Transform parent, IDeckData deck, Action<IDeck> onClick = null) =>
                   BuildDeckUI(parent, new Deck(deck, onClick));

    private Deck BuildDeckUI(Transform parent, Deck deck)
    {
        DeckUI deckUI;

        if (buffer.Count > 0) deckUI = buffer.Pop();
        else deckUI = DeckUI.CreatPrefab();

        deckUI.Build(parent, deck, Buffered);

        return deck;
    }

    private void Buffered(DeckUI deckUI)
    {
        buffer.Push(deckUI);
    }
}


