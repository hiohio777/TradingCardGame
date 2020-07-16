using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DecksPanel : BaseCollectionPanelUI, ICollectionPanelUI, IInitializable
{
    private IDeckFactory deckFactory;
    private IFractionsData fractions;
    private IUserData userDecks;
    private IEditorDeckPanel editorDeck;
    private List<IDeck> decks = new List<IDeck>();

    [SerializeField] private Transform panel = null;
    [SerializeField] private Button newDeckButton = null;
    [SerializeField] private int maxDeck = 4;
    private Transform parent;

    [Inject]
    public void InjectMetod(IDeckFactory deckFactory, IFractionsData fractions,
        IUserData userDecks, IEditorDeckPanel editorDeck)
    {
        (this.deckFactory, this.fractions, this.userDecks, this.editorDeck)
        = (deckFactory, fractions, userDecks, editorDeck);
    }

    public void Initialize()
    {

    }

    public override void Build(Transform parent)
    {
        this.parent = parent;
        base.Build(parent);
    }

    public override void Enable(FractionsMenu fractionMenu)
    {
        if (parent != null) parent.gameObject.SetActive(true);

        if (fractions.CurrentFraction.Name == "neutral")
            fractions.CurrentFraction = fractions.Fractions[0];

        base.Enable(fractionMenu);
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
        parent.gameObject.SetActive(false);

        editorDeck.StartEditDeck(() => Enable(fractionMenu));
    }

    private void SelectDeck(IDeck deck)
    {
        Disable();
        parent.gameObject.SetActive(false);

        editorDeck.StartEditDeck(deck.DeckData, () => Enable(fractionMenu));
    }

    private void OnEnable() => newDeckButton.onClick.AddListener(OnCreatNewDeck);
    private void OnDisable() => newDeckButton.onClick.RemoveListener(OnCreatNewDeck);
}
