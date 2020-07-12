using System;
using UnityEngine;

public interface IPanelUI
{
    MainMenuPanelsEnum TypePanel { get; }
    Transform TransformPanel{ get; }
    void SetParent(Transform parent);
    void Enable();
    void Disable();
}