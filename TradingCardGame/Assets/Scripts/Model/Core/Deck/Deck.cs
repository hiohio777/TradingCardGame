using System;

public class Deck : IDeck
{
    public event Action DestroyUI;
    public event Action<IDeck> Click;
    public event Action<bool> Select;

    public string Name { get => DeckData.Name; set => DeckData.Name = value; }
    public string Fraction { get => DeckData.Fraction; set => DeckData.Fraction = value; }
    public StatusDeckEnum Status { get => DeckData.Status; }

    public IDeckData DeckData { get; }

    private Deck() { }
    public Deck(IDeckData deckData, Action<IDeck> Select) => (DeckData, this.Click) = (deckData, Select);

    public void Destroy() => DestroyUI?.Invoke();
    public void OnClick() => Click.Invoke(this);
    public void OnSelect(bool isSelected) => Select.Invoke(isSelected);
}
