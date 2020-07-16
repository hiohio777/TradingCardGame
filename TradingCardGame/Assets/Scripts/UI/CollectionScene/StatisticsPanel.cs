using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatisticsPanel : BaseCollectionPanelUI, ICollectionPanelUI, IInitializable
{
    [SerializeField] private Text countVictory, countDefeat, countSeriesVictories;
    private IStatistics statistics;

    [Inject]
    public void InjectMetod(IStatistics statistics)
    {
        this.statistics = statistics;
    }

    public void Initialize()
    {

    }

    sealed public override void Enable(FractionsMenu fractionMenu)
    {
        gameObject.SetActive(true);

        countVictory.text = statistics.CountVictory.ToString();
        countDefeat.text = statistics.CountDefeat.ToString();
        countSeriesVictories.text = statistics.CountSeriesVictories.ToString();
    }
}
