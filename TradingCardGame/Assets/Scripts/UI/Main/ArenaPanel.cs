using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ArenaPanel : PanelUI, IPanelUI
{
    private IUserData user;
    private TrainingBattel.Factory trainingFactory;
    private ReturnButton returnButton;

    [SerializeField] private List<BattelMenuButton> menuButtons = null;

    [Inject]
    public void InjectMetod(ReturnButton returnButton, IUserData user, TrainingBattel.Factory trainingFactory)
    {
        this.user = user;
        this.trainingFactory = trainingFactory;
        this.returnButton = returnButton;

        menuButtons.ForEach(x => x.SetListener(OnSelectBattel));
    }

    protected override void Initialize() { }

    private void OnSelectBattel(object sender, TypeBattelEnum typeBattel)
    {
        if (user.Decks.Count == 0)
        {
            MessagePanel.SimpleMessage(transform, "no_decks");
            return;
        }

        user.CurrentTypeBattel = typeBattel;

        BaseBattel battel = null;
        switch (typeBattel)
        {
            case TypeBattelEnum.training:
                battel = trainingFactory.Create();
                break;
            case TypeBattelEnum.common:
                break;
            case TypeBattelEnum.rating:
                break;
            default:
                break;
        }
        battel.ReturnBack += Battel_ReturnBack;
        returnButton.SetActive(false);
        Disable();
    }

    private void Battel_ReturnBack()
    {
        Enable();
    }

    public override void Enable()
    {
        base.Enable();
        returnButton.SetActive(true);
    }
}
