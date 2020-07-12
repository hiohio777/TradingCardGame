using System;

public interface ICombatCard : ICardDataBase
{
    ICardData CardData { get; }
    IAttackCard AttackCard { get; set; }
    IBattelCombatData RoundData { get; set; }
    int CountRound { get; set; }
    bool ClassAura { get; set; }
    TypeDamegeEnum TypeReceivedDamege { get; }

    bool StandardDamage(IBattelCard enemy);
    bool DefenseDamage(IBattelCard enemy);
    bool TokenDamege(int damage);

    bool BuffInitiative(int initiative);
    bool BuffAttack(int attack);
    bool BuffDefense(int defense);
    bool BuffHealth(int health);
}
