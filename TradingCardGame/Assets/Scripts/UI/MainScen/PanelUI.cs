using System;
using UnityEngine;

public abstract class PanelUI : MonoBehaviour
{
    public event Action<PanelNameEnum> OpenSubPanel;

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
        Disable();
    }

    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }

    protected void OnOpenSubPanel(PanelNameEnum Name)
    {
        OpenSubPanel.Invoke(Name);
    }
}