using System;

public interface IDeck
{
    event Action<IDeck> Click;
    event Action<bool> Select;
    event Action DestroyUI; // Уничтожает Обьект на экране
    void OnClick();
    void OnSelect(bool isSelected);
    void Destroy();
    string Name { get; set; }
    string Fraction { get; set; }
    StatusDeckEnum Status { get; }
    IDeckData DeckData { get; }
}
