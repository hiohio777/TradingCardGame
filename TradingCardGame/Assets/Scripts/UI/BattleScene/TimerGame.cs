using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TimerGame : MonoBehaviour
{
    [SerializeField] private Text textTimer = null;
    [SerializeField] private float tickTime = 1;
    private int seconds, minutes;

    public void StartTimer()
    {
        StartCoroutine(RunTimer());

        seconds--;
        IncreaseSeconds();
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        seconds = minutes = 0;
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
            string strMin = minutes < 10 ? $"0{minutes}" : $"{minutes}";
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

            yield return null;
        }
    }
}
