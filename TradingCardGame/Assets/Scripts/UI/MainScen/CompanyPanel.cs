using Zenject;

public class CompanyPanel : PanelUI, IPanelUI
{

    [Inject]
    public void InjectMetod()
    {
    }

    protected override void Initialize() { }
}
