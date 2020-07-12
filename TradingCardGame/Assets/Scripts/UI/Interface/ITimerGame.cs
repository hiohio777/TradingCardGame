using System;

public interface ITimerGame
{
    void StartTimer(int count = 2000, Action execute = null);
    void StopTimer(bool isExecute = true);
}