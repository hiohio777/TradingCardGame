using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class PanelUI : MonoBehaviour, IPanelUI
{
    public MainMenuPanelsEnum TypePanel => typePanel;
    public Transform TransformPanel => transform;
    [SerializeField] private MainMenuPanelsEnum typePanel;

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
        Disable();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

}