using System;
using System.Collections.Generic;
using UnityEngine;

public class BattelCardFactory : FactoryBase<CardUI>, ICardFactory<IAttackCard>
{
    private readonly IAbilityFactory abilityFactory;
    private readonly ISpecificityFactory specificityFactory;
    private readonly IBuffUIParametersFactory buffUIFactory;

    public BattelCardFactory(ISpecificityFactory specificityFactory, IAbilityFactory abilityFactory, IBuffUIParametersFactory buffUIFactory) =>
        (this.abilityFactory, this.specificityFactory, this.buffUIFactory) = (abilityFactory, specificityFactory, buffUIFactory);

    public List<IAttackCard> GetCards(List<ICardData> cardsData) => GetCards(cardsData, new Vector3(1, 1, 1));
    public IAttackCard GetCard(ICardData cardData) => GetCard(cardData, new Vector3(1, 1, 1));
    public List<IAttackCard> GetCards(List<ICardData> cardsData, Vector3 scale)
    {
        var cards = new List<IAttackCard>();
        foreach (var item in cardsData)
            cards.Add(GetCard(item, scale));

        return cards;
    }

    public IAttackCard GetCard(ICardData cardData, Vector3 scale)
    {
        CardUI battelCardUI;

        if (buffer.Count > 0) battelCardUI = buffer.Pop();
        else
        {
            battelCardUI = UnityEngine.Object.Instantiate(Resources.Load<CardUI>($"Card/CardUI")).Initialize(Buffered, specificityFactory);
            battelCardUI.combatUI.buffUIFactory = buffUIFactory;
        }

        ICombatCard combat;
        battelCardUI.ability = abilityFactory.GetAbility(cardData.LinkAbility, battelCardUI.TransformCard);
        switch (cardData.Class.Type)
        {
            case ClassCardEnum.moon:
                combat = new CombatCardMoon(battelCardUI.combatUI, cardData, battelCardUI.StartSpecificity); break;
            case ClassCardEnum.sun:
                combat = new CombatCardSun(battelCardUI.combatUI, cardData, battelCardUI.StartSpecificity); break;
            default:
                combat = new CombatCardСommon(battelCardUI.combatUI, cardData, battelCardUI.StartSpecificity); break;
        }

        battelCardUI.BuildBattelCard(cardData, scale, combat);
        return battelCardUI as IAttackCard;
    }
}