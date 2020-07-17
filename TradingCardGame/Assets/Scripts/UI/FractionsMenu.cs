using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FractionsMenu : MonoBehaviour
{
    private Action<IFraction> clickHandler;
    private readonly List<FractionsButton> buttons = new List<FractionsButton>();
    private FractionsButton currentButton;

    [Inject]
    public void Initialize(IFractionsData fractions)
    {
        foreach (var item in fractions.Fractions)
        {
            var button = Instantiate(Resources.Load<FractionsButton>("FractionsButton"));
            button.Assing(transform, item, Select);
            buttons.Add(button);
        }
    }

    public FractionsMenu SetActiveBattons(List<IFraction> fractions)
    {
        foreach (var btn in buttons)
        {
            btn.SetActive(false);
            foreach (var fraction in fractions)
                if (fraction == btn.Identifier) btn.SetActive(true);
        }
        return this;
    }

    public void SetListener(Action<IFraction> clickHandler) =>
        this.clickHandler = clickHandler;

    public void SetSelecedButton(IFraction fraction)
    {
        foreach (var item in buttons)
            if (item.Identifier == fraction)
            {
                currentButton?.SelectFraction(false);
                currentButton = null;
                Select(item);
            }
    }

    private void Select(FractionsButton button)
    {
        if (button == currentButton) return;

        currentButton?.SelectFraction(false);
        (currentButton = button).SelectFraction(true);
        clickHandler?.Invoke(currentButton.Identifier);
    }
}
