public interface ICardDataBase
{
    string Name { get; }
    string Description { get; }
    IFraction Fraction { get; }
    IClassCard Class { get; }
    int Initiative { get; }
    int Attack { get; }
    int Defense { get; }
    int Health { get; }
    int MaxCountAttackers { get; }
}