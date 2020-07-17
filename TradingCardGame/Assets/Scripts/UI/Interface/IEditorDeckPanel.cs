using System;

public interface IDeckEditorPanel
{
    void StartEditDeck(IDeckData deck, Action back);
    void StartEditDeck(Action back);
}