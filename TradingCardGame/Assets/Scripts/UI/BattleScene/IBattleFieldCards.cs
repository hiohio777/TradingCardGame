using System.Collections.Generic;

public interface IBattleFieldCards
{
    List<IAttackCard> GetAttackingCardsEnemy { get; }
    List<IAttackCard> GetAttackingCardsPlayer { get; }
}