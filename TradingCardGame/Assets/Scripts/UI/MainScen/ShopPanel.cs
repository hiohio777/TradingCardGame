using Zenject;

public class ShopPanel : PanelUI, IPanelUI
{

    [Inject]
    public void InjectMetod()
    {
    }

    protected override void Initialize() { }
}