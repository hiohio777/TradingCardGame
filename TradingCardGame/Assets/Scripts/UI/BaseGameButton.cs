using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseGameButton<T> : MonoBehaviour
{
    private Action<T> clicKButton;
    public T Identifier { get => identifier; set => identifier = value; }
    [SerializeField] private T identifier;

    public void SetActive(bool active) => gameObject.SetActive(active);
    public void SetListener(Action<T> clicKButton) => this.clicKButton = clicKButton;
    protected virtual void Awake() => GetComponent<Button>().onClick.AddListener(OnClick);
    protected virtual void OnClick() => clicKButton?.Invoke(identifier);
}
