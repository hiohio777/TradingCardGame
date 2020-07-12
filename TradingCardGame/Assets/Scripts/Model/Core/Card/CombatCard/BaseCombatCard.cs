using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public abstract class BaseCombatCard : ICombatCard
{
    protected readonly ICardUIParameters parameters;
    protected readonly Action<TypeSpecificityEnum> startSpecificity;
    protected const int max = 60;

    protected int initiative, attack, defense, health;
    protected int maxCountAttackers;

    public BaseCombatCard(ICardUIParameters parameters, ICardData cardData, Action<TypeSpecificityEnum> startSpecificity) =>
        (this.parameters, CardData, initiative, attack, defense, health, maxCountAttackers, this.startSpecificity) =
        (parameters, cardData, cardData.Initiative, cardData.Attack, cardData.Defense,
        cardData.Health, cardData.MaxCountAttackers, startSpecificity);

    public ICardData CardData { get; private set; }
    public IAttackCard AttackCard { get; set; }
    public IBattelCombatData RoundData { get; set; }

    public string Name => CardData.Name;
    public string Description => CardData.Description;
    public IFraction Fraction => CardData.Fraction;
    public IClassCard Class => CardData.Class;
    public bool ClassAura { get; set; } = true;

    public int Initiative { get => initiative; set { parameters.ShowInitiative(value, value - initiative); initiative = value; } }
    public int Attack { get => attack; set { parameters.ShowAttack(value, value - attack); attack = value; } }
    public int Defense { get => defense; set {parameters.ShowDefense(value, value - defense); defense = value; } }
    public int Health { get => health; set { parameters.ShowHealth(value, value - health); health = value; } }
    public int MaxCountAttackers { get => maxCountAttackers; set => maxCountAttackers = value; }
    public int CountRound { get; set; } = 0;
    public TypeDamegeEnum TypeReceivedDamege { get; private set; } 

    public bool BuffInitiative(int initiativeBaff)
    {
        if (initiativeBaff == 0 || (initiativeBaff > 0 && initiative == 8) || (initiativeBaff < 0 && initiative == 1))
            return false;
        Initiative = math.clamp(initiative + initiativeBaff, 1, 8);
        return true;
    }

    public bool BuffAttack(int attackBaff)
    {
        if (attackBaff == 0 || (attackBaff > 0 && attack == max) || (attackBaff < 0 && attack == 1))
            return false;
        Attack = math.clamp(attack + attackBaff, 1, max);
        return true;
    }

    public bool BuffDefense(int defenseBaff)
    {
        if (defenseBaff == 0 || (defenseBaff > 0 && defense == max) || (defenseBaff < 0 && defense == 0))
            return false;
        Defense = math.clamp(defense + defenseBaff, 0, max);
        return true;
    }

    public bool BuffHealth(int healthBaff)
    {
        if (healthBaff == 0 || (healthBaff > 0 && health == max) || (healthBaff < 0 && health == -max))
            return false;
        Health = math.clamp(health + healthBaff, -max, max);

        if (healthBaff < 0) AbilityDamage();
        return true;
    }

    public virtual bool StandardDamage(IBattelCard enemy)
    {
        Health -= enemy.Combat.Attack;

        TypeReceivedDamege = TypeDamegeEnum.Standard;
        RoundData.ResponsekDamageCards.Add(AttackCard);
        return true;
    }

    public virtual bool DefenseDamage(IBattelCard enemy)
    {
        Health -= enemy.Combat.Defense;

        TypeReceivedDamege = TypeDamegeEnum.Defense;
        RoundData.ResponsekDamageCards.Add(AttackCard);
        return true;
    }

    public virtual bool TokenDamege(int damage)
    {
        Health -= damage;

        TypeReceivedDamege = TypeDamegeEnum.Token;
        RoundData.ResponsekDamageCards.Add(AttackCard);
        return true;
    }

    private bool AbilityDamage()
    {
        TypeReceivedDamege = TypeDamegeEnum.Ability;
        RoundData.ResponsekDamageCards.Add(AttackCard);
        return true;
    }
}