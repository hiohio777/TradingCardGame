using System;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour, IAbility
{
    [SerializeField, Header("Фазы срабатывания")] private List<EventTriggerEnum> triggers = new List<EventTriggerEnum>();
    [SerializeField, Space(5), Header("Одноразовая")] private bool oneOff = false;
    [SerializeField, Space(2)] private TypeSpecificityEnum specificity = TypeSpecificityEnum.Default;

    [SerializeField, Space(10)] private ConditionsAbility conditionsAbility = null;
    [SerializeField] private EffectAbility effectAbility = null;
    [SerializeField] private float timeBefore = 0.5f, timeAfte = 0.5f, timespeed = 0.1f;

    public ISFXFactory specificityFactory;

    private Action finish;
    private bool isReady = true;

    public List<EventTriggerEnum> TypeTriggers => triggers;
    public void Destroy() => Destroy(gameObject);

    public void TriggerEvent(EventTriggerEnum requestTrigger, IAttackCard card, IBattelBase battel, Action finish)
    {
        this.finish = finish;

        if (isReady == false || triggers.Contains(requestTrigger) == false || conditionsAbility.IsConditions(card, battel) == false)
            finish?.Invoke();
        else
            Finish(card, effectAbility.IsResult(card, battel, specificityFactory));
    }

    private void Finish(IAttackCard card, bool result)
    {
        if (result)
        {
            if (oneOff) isReady = false;
            card.ImplementAbility(specificity, finish);
        }
        else finish?.Invoke();
    }
}