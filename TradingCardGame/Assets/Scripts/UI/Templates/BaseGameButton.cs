using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseGameButton<T> : MonoBehaviour
{
    private Action<BaseGameButton<T>, T> clicKButton;
    public T Identifier { get => identifier; set => identifier = value; }
    [SerializeField] private T identifier;
    [HideInInspector] public Button button;

    public virtual void SetActive(bool active) => gameObject.SetActive(active);
    public void SetListener(Action<BaseGameButton<T>, T> clicKButton) => this.clicKButton = clicKButton;
    protected virtual void OnClick() => clicKButton?.Invoke(this, identifier);

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
}

public abstract class BaseGameButton : MonoBehaviour
{
    private Action<BaseGameButton> clicKButton;
    [HideInInspector] public Button button;
    public bool Interactable { get => button.interactable; set => button.interactable = value; }

    public virtual void SetActive(bool active) => gameObject.SetActive(active);
    public void SetListener(Action<BaseGameButton> clicKButton) => this.clicKButton = clicKButton;
    public void ClearListener() => this.clicKButton = null;
    protected virtual void OnClick() => clicKButton?.Invoke(this);

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
}




