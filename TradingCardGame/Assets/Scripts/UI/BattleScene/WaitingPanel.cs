using UnityEngine;

public class WaitingPanel : MonoBehaviour
{
    [SerializeField] private GameObject waitConnectionPanel;
    [SerializeField] private GameObject waitingEnemyPanel;
    private TimerGame timer;

    private void Awake()
    {
        timer = GetComponent<TimerGame>();
        waitConnectionPanel.gameObject.SetActive(false);
        waitingEnemyPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void StartWaitConnection()
    {
        gameObject.SetActive(true);
        waitConnectionPanel.gameObject.SetActive(true);
        timer.StartTimer();
    }

    public void StartWaitingEnemy()
    {
        waitConnectionPanel.gameObject.SetActive(false);
        waitingEnemyPanel.gameObject.SetActive(true);
        timer.StartTimer();
    }

    public void DestroyUI()
    {
        Destroy(gameObject);
        //battel.Enemy.Creat("ENEMY_123", "spawn", listCards);
    }
}