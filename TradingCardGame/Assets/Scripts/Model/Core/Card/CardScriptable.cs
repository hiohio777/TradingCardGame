using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "CardScriptable", menuName = "Data/Card", order = 51)]
public class CardScriptable : ScriptableObject, ICardData
{
    [SerializeField, Space(10)] private Sprite icon = null;
    [SerializeField] private FractionScriptable fraction = null;
    [SerializeField, Space(10)] private ClassCardScriptable classCard = null;
    [SerializeField, Space(10)] private GameObject abilityPrefab = null;

    [SerializeField] private Combat combatData = null;

    public Sprite Icon => icon;
    public string Name => name;
    public string Description => $"{name}_description";
    public IFraction Fraction => fraction;
    public IClassCard Class => classCard;

    public int Initiative => combatData.initiative;
    public int Attack => combatData.attack;
    public int Defense => combatData.defense;
    public int Health => combatData.health;
    public int MaxCountAttackers => combatData.maxCountAttackers;
    public bool IsOpen { get; private set; }
    public TypeInitiativeEnum TypeInitiative { get; private set; }
    public GameObject LinkAbility => abilityPrefab;

    public ICardData Open()
    {
        IsOpen = true;
        return this;
    }

    private void OnEnable()
    {
        if (Initiative <= 2) TypeInitiative = TypeInitiativeEnum.veryFast;
        else if (Initiative <= 4) TypeInitiative = TypeInitiativeEnum.fast;
        else if (Initiative <= 6) TypeInitiative = TypeInitiativeEnum.slow;
        else if (Initiative <= 8) TypeInitiative = TypeInitiativeEnum.verySlow;

        if (fraction == null)
        {
            Debug.LogError($"Card: {name} -> No faction assigned!");
            fraction = Resources.Load<FractionScriptable>("Data/Fractions/neutral");
        }

        if (classCard == null)
        {
            Debug.LogError($"Card: {name} -> No class assigned!");
            fraction = Resources.Load<FractionScriptable>("Data/ClassCard/sun");
        }

        // TEST
        combatData.attack = 5;
        combatData.defense = 3;
        combatData.health = 14;
    }
}
