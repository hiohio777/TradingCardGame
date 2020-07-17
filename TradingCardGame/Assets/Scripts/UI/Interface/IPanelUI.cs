using System;
using UnityEngine;

public interface IPanelUI
{
    event Action<object, PanelNameEnum> OpenSubPanel;
    PanelNameEnum Name { get; }

    void SetParent(Transform parent);
    void Enable();
    void Disable();
}