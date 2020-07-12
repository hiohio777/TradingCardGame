using System;
using UnityEngine;

public interface IEditorDeckPanel
{
    void StartEditDeck(IDeckData deck, Action back);
    void StartEditDeck(Action back);
}