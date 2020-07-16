using Zenject;

public class CompanyPanel : PanelUI, IPanelUI
{
    public PanelNameEnum Name { get; } = PanelNameEnum.Company;

    [Inject]
    public void InjectMetod()
    {
    }
}
