using System;
using UnityEngine;
using UnityEngine.UI;

public class CollectionMenuButton : BaseObjectCklic<PanelNameEnum>
{
    private Image image;
    public override void SetActive(bool active)
    {
        if (active) image.color = Color.green;
        else image.color = Color.gray;
    }

    private void Start()
    {
        image = GetComponent<Image>();
        SetActive(false);
    }
}
