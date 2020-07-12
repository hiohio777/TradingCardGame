using UnityEngine;

public class BaseCollectionPanelUI : MonoBehaviour, ICollectionPanelUI
{
    public CollectionPanelsEnum TypePanel { get => typePanel; }
    [SerializeField] private CollectionPanelsEnum typePanel;
    protected FractionsMenu fractionMenu;

    private void Awake()
    {
        Disable();
    }

    public virtual void Build(Transform parent)
    {
        transform.SetParent(parent, false);
        gameObject.SetActive(false);
    }

    public virtual void Enable(FractionsMenu fractionMenu)
    {
        this.fractionMenu = fractionMenu;
        fractionMenu.transform.SetParent(transform, false);
        gameObject.SetActive(true);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}