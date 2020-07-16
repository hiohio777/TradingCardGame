using UnityEngine;
using Zenject;

public class CardsCollectionPanel : BaseCollectionPanelUI, ICollectionPanelUI, IInitializable
{
    private IFractionsData fractions;
    private ICollectionCardsData collection;
    private ICardFactory<ICard> cardFactory;

    [SerializeField] private CardsPanelUI cardsPanel = null;

    [Inject]
    public void InjectMetod(ICardFactory<ICard> cardFactory, ICollectionCardsData collection, IFractionsData fractions)
    {
        (this.cardFactory, this.collection, this.fractions) = (cardFactory, collection, fractions);
    }

    public void Initialize()
    {
        cardsPanel.Build(cardFactory, collection, OnClickCard, fractions);
    }

    public override void Enable(FractionsMenu fractionMenu)
    {
        base.Enable(fractionMenu);
        fractionMenu.SetActiveBattons(fractions.Fractions).SetListener(SelectFraction);
        fractionMenu.SetSelecedButton(fractions.CurrentFraction);
    }

    public override void Disable()
    {
        cardsPanel.DestroyCardUI();
        base.Disable();
    }

    private void SelectFraction(object fraction) =>
         cardsPanel.SelectFraction(fraction as IFraction);

    private void OnClickCard(ICard card) { }
}