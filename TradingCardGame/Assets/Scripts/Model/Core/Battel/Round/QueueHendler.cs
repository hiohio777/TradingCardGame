using System;
using System.Collections.Generic;

public abstract class QueueHendler
{
    public IBattelBase battel;
    protected List<IAttackCard> cards;
    protected Action actionFinish;
    protected EventTriggerEnum triggerAbility;

    protected QueueHendler(IBattelBase battel, Action actionFinish) =>
        (this.battel, this.actionFinish) = (battel, actionFinish);

    public virtual void Execute()
    {
        cards = GeneralFunctionsRound.CreatQueue(battel);
        RunQueue();
    }

    protected void RunQueue(IAttackCard current = null)
    {
        if (current != null)
            cards.Remove(current);

        if (cards.Count != 0)
        {
            current = null;
            foreach (var card in cards)
                if (current == null || current.BattelCard.Combat.Initiative > card.BattelCard.Combat.Initiative
                || (current.BattelCard.Combat.Initiative == card.BattelCard.Combat.Initiative && current.Fortune == false))
                    current = card;
            Next(current);
        }
        else Finish();
    }

    protected virtual void Next(IAttackCard current) =>
        current.ExecuteAbility(triggerAbility, battel, () => { CheckForDeath(current); });
    protected abstract void Finish();

    private void CheckForDeath(IAttackCard current) =>
        new DeathCards(battel, () => RunQueue(current), cards).Execute();
}
