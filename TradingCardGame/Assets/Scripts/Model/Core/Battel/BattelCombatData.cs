using System.Collections.Generic;

public class BattelCombatData : IBattelCombatData
{
    public BattelCombatData() =>
        (ResponsekDamageCards) = (new List<IAttackCard>());

    public List<IAttackCard> ResponsekDamageCards { get; set; }
}
