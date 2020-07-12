using UnityEngine;
using UnityEngine.UI;

public class StatisticsPanel : BaseCollectionPanelUI, ICollectionPanelUI
{
    [SerializeField] private Text countVictory, countDefeat, countSeriesVictories;
    private IStatistics statistics;

    public ICollectionPanelUI Initialize(IStatistics statistics)
    {
        this.statistics = statistics;
        return this;
    }

    sealed public override void Enable(FractionsMenu fractionMenu)
    {
        gameObject.SetActive(true);

        countVictory.text = statistics.CountVictory.ToString();
        countDefeat.text = statistics.CountDefeat.ToString();
        countSeriesVictories.text = statistics.CountSeriesVictories.ToString();
    }
}
