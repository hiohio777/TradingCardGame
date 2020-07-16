using Zenject;

public class ShopPanel : PanelUI, IPanelUI
{
    public PanelNameEnum Name { get; } = PanelNameEnum.Shop;

    [Inject]
    public void InjectMetod()
    {
    }
}