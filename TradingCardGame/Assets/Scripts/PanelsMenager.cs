using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PanelsMenager : MonoBehaviour
{
    private List<IPanelUI> panels;
    private readonly Stack<IPanelUI> panelsStack = new Stack<IPanelUI>();
    private BaseGameButton<bool> returnButton;

    [Inject]
    public void InjectMetod(List<IPanelUI> panels, BaseGameButton<bool> returnButton)
    {
        (this.panels, this.returnButton) = (panels, returnButton);
        panels.ForEach(x => Customize(x));
        returnButton.SetListener(ToReturn);
        returnButton.transform.SetParent(transform, false);
        returnButton.transform.SetAsLastSibling();
        returnButton.SetActive(false);
    }

    public void OpenPanel(PanelNameEnum childName)
    {
        if (panelsStack.Count > 0)
            panelsStack.Peek().Disable();
        foreach (var item in panels)
        {
            if (item.Name == childName)
            {
                panelsStack.Push(item);
                returnButton.SetActive(true);
                item.Enable();
                break;
            }
        }
    }

    private void Customize(IPanelUI panel)
    {
        panel.SetParent(transform);
        panel.OpenSubPanel += OpenPanel;
        panel.Disable();
    }

    private void ToReturn(bool s)
    {
        panelsStack.Pop().Disable();
        panelsStack.Peek().Enable();
    }

    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "Default";
    }

    private void OnDisable()
    {
        panels.ForEach(x => x.OpenSubPanel -= OpenPanel);
    }
}
