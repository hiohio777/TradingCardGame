using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDeckFactory
{
    void ClearBuffer();
    List<IDeck> GetDecks(Transform parent, List<IDeckData> decks, Action<IDeck> onClick = null, int maxDeck = -1);
    IDeck GetDeck(Transform parent, IDeckData deck, Action<IDeck> onClick = null);
}
