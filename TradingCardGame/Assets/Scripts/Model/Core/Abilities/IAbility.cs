using System;
using System.Collections.Generic;

public interface IAbility
{
    List<EventTriggerEnum> TypeTriggers { get; }
    void TriggerEvent(EventTriggerEnum trigger, IAttackCard card, IBattelBase battel, Action finish);
    void Destroy();
}