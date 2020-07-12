using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : IStatisticsBattele, IStatistics
{
    public int CountVictory { get; private set; }
    public int CountDefeat { get; private set; }
    public int CountSeriesVictories { get; private set; }

    public void DeclareVictory()
    {
        CountVictory++;
        CountSeriesVictories++;

        SendResultsServer();
    }

    public void DeclareDefeat()
    {
        CountDefeat++;
        CountSeriesVictories = 0;

        SendResultsServer();
    }

    private void SendResultsServer()
    {
        Debug.Log($"Данные о статистике отправлены на сервер!");
    }
}
