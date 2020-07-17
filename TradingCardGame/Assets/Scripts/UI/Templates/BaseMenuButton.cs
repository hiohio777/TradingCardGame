using System;
using System.Collections.Generic;
using UnityEngine;
// Меню выбора одной активной кнопки из представленных(buttons)
public abstract class BaseMenuButton<T, U> : MonoBehaviour where T : BaseGameButton<U>
{
    [SerializeField] private List<T> buttons = null;
    private BaseGameButton<U> currentButton;
    private Action<BaseMenuButton<T, U>, U> clicKButton;

    public void SetListener(Action<BaseMenuButton<T, U>, U> clicKButton)
    {
        this.clicKButton = clicKButton;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
    }

    private void Awake()
    {
        buttons.ForEach(x => x.SetListener(OnClick));
    }

    private void OnClick(BaseGameButton<U> sender, U arg)
    {
        currentButton?.SetActive(false);
        (currentButton = sender).SetActive(true);

        clicKButton.Invoke(this, arg);
    }
}
