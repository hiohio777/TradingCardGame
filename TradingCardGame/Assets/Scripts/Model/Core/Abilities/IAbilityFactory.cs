using UnityEngine;

public interface IAbilityFactory
{
    IAbility GetAbility(GameObject abilityPrefab, Transform parent);
}