using System.Collections.Generic;
using UnityEngine;

public class BattelCardFactory : FactoryBase<Card>, ICardFactory<IAttackCard>
{
    private readonly IAbilityFactory abilityFactory;
    private readonly ISFXFactory specificityFactory;
    private readonly IBuffUIParametersFactory buffUIFactory;


    public BattelCardFactory(ISFXFactory specificityFactory, IAbilityFactory abilityFactory, IBuffUIParametersFactory buffUIFactory) =>
        (this.abilityFactory, this.specificityFactory, this.buffUIFactory) = (abilityFactory, specificityFactory, buffUIFactory);

    public List<IAttackCard> GetCards(List<ICardData> cardsData)
        => GetCards(cardsData, new Vector3(1, 1, 1));
    public IAttackCard GetCard(ICardData cardData)
        => GetCard(cardData, new Vector3(1, 1, 1));
    public List<IAttackCard> GetCards(List<ICardData> cardsData, Vector3 scale)
    {
        var cards = new List<IAttackCard>();
        foreach (var item in cardsData)
            cards.Add(GetCard(item, scale));

        return cards;
    }

    public IAttackCard GetCard(ICardData cardData, Vector3 scale)
    {
        AttackCard card;

        if (buffer.Count > 0) card = buffer.Pop() as AttackCard;
        else
        {
            card = UnityEngine.Object.Instantiate(Resources.Load<AttackCard>($"Card/AttackCard"));
            card.Initial(Buffered, specificityFactory);
            card.combatUI.buffUIFactory = buffUIFactory;
            conteiner.Add(card);
        }

        ICombatCard combat;
        var ability = abilityFactory.GetAbility(cardData.LinkAbility, card.View.CardTransform);
        switch (cardData.Class.Type)
        {
            case ClassCardEnum.moon:
                combat = new CombatCardMoon(card.combatUI, cardData, card.StartSFX); break;
            case ClassCardEnum.sun:
                combat = new CombatCardSun(card.combatUI, cardData, card.StartSFX); break;
            default:
                combat = new CombatCardСommon(card.combatUI, cardData, card.StartSFX); break;
        }

        combat.AttackCard = card;
        card.Build(cardData, scale, combat, ability);
        return card;
    }
}