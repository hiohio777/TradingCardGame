using System;
using UnityEngine;

public class AbilityFactory : IAbilityFactory
{
    private readonly ISpecificityFactory specificityFactory;

    public AbilityFactory(ISpecificityFactory specificityFactory) =>
        this.specificityFactory = specificityFactory ?? throw new ArgumentNullException(nameof(specificityFactory));

    public IAbility GetAbility(GameObject abilityPrefab, Transform parent)
    {
        var ability = UnityEngine.Object.Instantiate(abilityPrefab).GetComponent<Ability>();
        ability.transform.SetParent(parent, false);
        ability.specificityFactory = specificityFactory;
        return ability;
    }
}
