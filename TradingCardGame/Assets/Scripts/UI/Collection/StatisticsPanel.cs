using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatisticsPanel : PanelUI, IPanelUI
{
    [SerializeField] private Text countVictory, countDefeat, countSeriesVictories;
    private IStatistics statistics;
    private CollectionMenu menu;

    [Inject]
    public void InjectMetod(IStatistics statistics, CollectionMenu menu) =>
        (this.statistics, this.menu) = (statistics, menu);

    protected override void Initialize()
    {
        countVictory.text = statistics.CountVictory.ToString();
        countDefeat.text = statistics.CountDefeat.ToString();
        countSeriesVictories.text = statistics.CountSeriesVictories.ToString();
    }

    public override void Enable()
    {
        base.Enable();

        menu.transform.SetParent(transform, false);
    }
}
