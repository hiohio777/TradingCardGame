using System;
using UnityEngine;

public abstract class PanelUI : MonoBehaviour, IPanelUI
{
    public PanelNameEnum Name => namePanel;
    [SerializeField] protected PanelNameEnum namePanel;
    public event Action<object, PanelNameEnum> OpenSubPanel;
    private bool isCreatPanal = false;

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
        Disable();
    }

    public virtual void Enable()
    {
        if (isCreatPanal == false)
        {
            Initialize();
            isCreatPanal = true;
        }

        gameObject.SetActive(true);
    }

    protected abstract void Initialize();

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }

    protected void OnOpenSubPanel(object sender, PanelNameEnum Name)
    {
        OpenSubPanel.Invoke(sender, Name);
    }
}