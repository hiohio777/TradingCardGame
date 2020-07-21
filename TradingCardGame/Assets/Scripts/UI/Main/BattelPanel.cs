using Zenject;

public class BattelPanel : PanelUI, IPanelUI
{
    public IBattel Battel { get; private set; }
    public IUserData UserData { get; private set; }
    protected BattelFieldFactory battelFieldFactory;
    protected bool isStartBattel = false;

    [Inject]
    public void InjectMetod()
    {
    }

    protected override void Initialize()
    { 
    
    }

    private void OnDisable()
    {
        // Уничтожить все обьекты и UI битвы
    }
}