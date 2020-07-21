using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectionPanel : PanelUI, IPanelUI
{
    private IDeckFactory deckFactory;
    private IFractionsData fractions;
    private IUserData userDecks;
    private IDeckEditorPanel editorDeck;
    private List<IDeck> decks = new List<IDeck>();
    private FractionsMenu fractionMenu;
    private CollectionMenu menu;

    [SerializeField] private Transform panel;
    [SerializeField] private Button newDeckButton;
    [SerializeField] private int maxDeck = 4;

    [Inject]
    public void InjectMetod(IDeckFactory deckFactory, IFractionsData fractions, CollectionMenu menu,
        IUserData userDecks, IDeckEditorPanel editorDeck, FractionsMenu fractionMenu) =>
        (this.deckFactory, this.fractions, this.menu, this.userDecks, this.editorDeck, this.fractionMenu)
        = (deckFactory, fractions, menu, userDecks, editorDeck, fractionMenu);

    protected override void Initialize()
    {
        menu.AssignButton(0);
        newDeckButton.onClick.AddListener(OnCreatNewDeck);
        fractionMenu.transform.SetAsLastSibling();
    }

    public override void Enable()
    {
        base.Enable();

        menu.transform.SetParent(transform, false);
        fractionMenu.transform.SetParent(transform, false);
        if (fractions.CurrentFraction.Name == "neutral")
            fractions.CurrentFraction = fractions.Fractions[0];
        fractionMenu.SetActiveBattons(fractions.Fractions.Where(x => x.Name != "neutral").ToList()).SetListener(SelectFraction);
        fractionMenu.SetSelecedButton(fractions.CurrentFraction);
    }

    private void SelectFraction(IFraction fraction)
    {
        fractions.CurrentFraction = fraction;

        decks.ForEach(x => x.Destroy());
        decks = deckFactory.GetDecks(panel, userDecks.GetFractionDeck(fractions.CurrentFraction.Name), SelectDeck, maxDeck);

        if (decks.Count < maxDeck) NewDeckButtonActive(true);
        else NewDeckButtonActive(false);
    }

    private void NewDeckButtonActive(bool active)
    {
        newDeckButton.gameObject.SetActive(active);
        newDeckButton.transform.SetAsLastSibling();
    }

    private void OnCreatNewDeck()
    {
        Disable();
        editorDeck.StartEditDeck(() => Enable());
    }

    private void SelectDeck(IDeck deck)
    {
        Disable();
        editorDeck.StartEditDeck(deck.DeckData, () => Enable());
    }
}
