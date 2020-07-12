using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerGame : MonoBehaviour
{
    [SerializeField] private Text textTimer;

    private int count, seconds, minutes;
    private Action execute;
    private bool isEndless;

    public void StartTimer(int count = 0, Action execute = null)
    {
        (this.count, this.execute, isEndless) = (count, execute, count <= 0);
        StartCoroutine(RunTimer());

        seconds--;
        IncreaseSeconds();
    }

    public void StopTimer(bool isExecute = true)
    {
        StopAllCoroutines();
        if (isExecute) execute?.Invoke();
    }

    private void IncreaseSeconds()
    {
        seconds++;
        if (seconds >= 60)
        {
            seconds = 0;
            minutes++;
        }

        if (textTimer != null)
        {
            string strMin = minutes < 10 ? $"0{minutes}": $"{minutes}";
            string strSec = seconds < 10 ? $"0{seconds}" : $"{seconds}";

            textTimer.text = $"{strMin} : {strSec}";
        }
    }

    private IEnumerator RunTimer()
    {
        float time = 0;
        float timeSecond = 0;

        while (true)
        {
            time += Time.deltaTime;
            timeSecond += Time.deltaTime;

            if (timeSecond > 1)
            {
                timeSecond = 0;
                IncreaseSeconds();
            }

            if (time > count && isEndless == false)
               StopTimer();
            yield return null;
        }
    }
}
