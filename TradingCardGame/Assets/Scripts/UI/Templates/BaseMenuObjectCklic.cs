using System;
using System.Collections.Generic;
using UnityEngine;
// Меню выбора одной активной кнопки из представленных(buttons)
public abstract class BaseMenuObjectCklic<T, U> : MonoBehaviour where T : BaseObjectCklic<U>
{
    public List<T> buttons = null;
    private BaseObjectCklic<U> currentButton;
    private Action<BaseMenuObjectCklic<T, U>, U> clicKButton;

    public void SetListener(Action<BaseMenuObjectCklic<T, U>, U> clicKButton)
    {
        this.clicKButton = clicKButton;
    }

    // Позволяент назначить какуюлибо кнопку активной из скрипта
    public void AssignButton(int index)
    {
        buttons[index].SetActive(true);
        currentButton?.SetActive(false);
        currentButton = buttons[index];
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
    }

    private void Awake()
    {
        buttons.ForEach(x => x.SetListener(OnClick));
    }

    private void OnClick(BaseObjectCklic<U> sender, U arg)
    {
        if (currentButton == sender)
            return;

        currentButton?.SetActive(false);
        (currentButton = sender).SetActive(true);

        clicKButton?.Invoke(this, arg);
    }
}
