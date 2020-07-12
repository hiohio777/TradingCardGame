using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FractionsButton : MonoBehaviour, IPointerClickHandler
{
    public IFraction Identifier { get; private set; }
    [SerializeField] private LocalisationText nameFraction = null;
    [SerializeField] private Image fon;

    private Action<FractionsButton> clickHandler;

    public FractionsButton Assing(Transform parent, IFraction fraction, Action<FractionsButton> click)
    {
        (Identifier, this.clickHandler) = (fraction, click);
        nameFraction.SetKey(fraction.Name);
        transform.SetParent(parent, false);
        fon.color = Color.gray;
        return this;
    }

    public void SelectFraction(bool active)
    {
        if (active)
        {
            fon.color = Color.yellow;
            return;
        }

        fon.color = Color.gray;
    }

    public void SetActive(bool active) => gameObject.SetActive(active);
    public void OnPointerClick(PointerEventData eventData) => clickHandler.Invoke(this);
}