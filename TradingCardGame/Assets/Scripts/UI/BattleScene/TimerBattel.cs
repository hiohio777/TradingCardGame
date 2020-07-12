using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerBattel : MonoBehaviour
{
    public event Action Execute, ExecuteTick;
    public event Action<bool> ExecuteBlockButton;

    [SerializeField] private Text textTimer = null;
    [SerializeField] private int countStart = 50;
    [SerializeField] private float tickTime = 1;

    private int currentTime;
    private bool isWorks;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        if (isWorks) return;

        ItitialData();
        StartCoroutine(RunTimer());
    }

    public void HideTimer()
    {
        if (isWorks == false) return;

        StopAllCoroutines();
        isWorks = false;
        gameObject.SetActive(false);
    }

    public void Tick()
    {
        if (isWorks == false)
            ItitialData();

        currentTime--;
        textTimer.text = currentTime.ToString();

        if (currentTime == 5) // Блокировать кнопку окончания хода, когда остаётся совсем мало времени
        {
            ExecuteBlockButton.Invoke(false);
            textTimer.color = Color.red;
        }

        if (currentTime <= 0)
        {
            Execute.Invoke();
            HideTimer();
        }
    }

    private void ItitialData()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        currentTime = countStart;
        textTimer.color = Color.blue;
        textTimer.text = currentTime.ToString();
        isWorks = true;
    }

    private IEnumerator RunTimer()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;
            if (time > tickTime)
            {
                time = 0;
                ExecuteTick?.Invoke();
            }

            yield return null;
        }
    }
}
