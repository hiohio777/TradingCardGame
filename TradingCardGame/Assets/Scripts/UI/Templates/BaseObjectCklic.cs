using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseObjectCklic<T> : MonoBehaviour, IPointerClickHandler
{
    private Action<BaseObjectCklic<T>, T> clicKButton;
    [SerializeField] private T identifier;
    public bool interactable = true;

    public virtual void SetActive(bool active) => gameObject.SetActive(active);
    public void SetListener(Action<BaseObjectCklic<T>, T> clicKButton) => this.clicKButton = clicKButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        clicKButton?.Invoke(this, identifier);
    }
}

public abstract class BaseObjectCklic : MonoBehaviour, IPointerClickHandler
{
    private Action<BaseObjectCklic> clicKButton;
    public bool interactable = true;

    public virtual void SetActive(bool active) => gameObject.SetActive(active);
    public void SetListener(Action<BaseObjectCklic> clicKButton) => this.clicKButton = clicKButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        clicKButton?.Invoke(this);
    }
}




