using UnityEngine;
using Zenject;

public class CardsCollectionPanel : PanelUI, IPanelUI
{
    private IFractionsData fractions;
    private ICollectionCardsData collection;
    private ICardFactory<ICard> cardFactory;
    private FractionsMenu fractionMenu;
    private CollectionMenu menu;

    [SerializeField] private CardsPanelUI cardsPanel = null;

    [Inject]
    public void InjectMetod(ICardFactory<ICard> cardFactory, ICollectionCardsData collection, IFractionsData fractions,
        FractionsMenu fractionMenu, CollectionMenu menu)
    {
        (this.cardFactory, this.collection, this.fractions, this.fractionMenu, this.menu)
        = (cardFactory, collection, fractions, fractionMenu, menu);
    }

    protected override void Initialize()
    {
        cardsPanel.Build(cardFactory, collection, OnClickCard, fractions);
    }

    public override void Enable()
    {
        base.Enable();

        menu.transform.SetParent(transform, false);
        fractionMenu.transform.SetParent(transform, false);
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