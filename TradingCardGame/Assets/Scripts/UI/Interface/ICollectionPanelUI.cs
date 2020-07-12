using UnityEngine;

public interface ICollectionPanelUI
{
    CollectionPanelsEnum TypePanel { get; }
    void Build(Transform parent);
    void Enable(FractionsMenu fractionMenu);
    void Disable();
}
