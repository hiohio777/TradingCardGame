using System;
using UnityEngine;
using UnityEngine.UI;

public class StartingHandPanel : MonoBehaviour, IStartingHandPanel
{
    [SerializeField] private Button buttonNewStartingHand = null, buttonAccept = null;
    [SerializeField] private int countAttempts;

    private IBattelPerson person;
    private Action<object> accept;
    private TimerBattel timerNextTurn;

    public StartingHandPanel Initialize(Transform parent, TimerBattel timerNextTurn, IBattelPerson person, Action<object> accept)
    {
        (this.timerNextTurn, this.person, this.accept) = (timerNextTurn, person, accept);
        transform.SetParent(parent, false);

        //Это необходимо, чтобы в случае истечения таймера в пвп битве интерфейс был уничтожен
        if (this.timerNextTurn != null)
            this.timerNextTurn.Execute += DestroyUI;

        buttonNewStartingHand.onClick.AddListener(OnNewStartingHand);
        buttonAccept.onClick.AddListener(OnAccept);

        OnNewStartingHand();

        return this;
    }

    public void OnNewStartingHand()
    {
        person.NewStartingHand();
        countAttempts--;

        if (countAttempts <= 0)
        {
            buttonNewStartingHand.gameObject.SetActive(false);
        }
    }

    private void OnAccept()
    {
        accept?.Invoke(this);
        DestroyUI();
    }

    private void DestroyUI(object sender = null)
    {
        if (this.timerNextTurn != null)
            timerNextTurn.Execute -= DestroyUI;
        Destroy(gameObject);
    }
}