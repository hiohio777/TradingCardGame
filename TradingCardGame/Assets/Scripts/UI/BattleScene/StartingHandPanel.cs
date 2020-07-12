using System;
using UnityEngine;
using UnityEngine.UI;

public class StartingHandPanel : MonoBehaviour, IStartingHandPanel
{
    [SerializeField] private Button buttonNewStartingHand = null, buttonAccept = null;
    [SerializeField] private int countAttempts;

    private IBattelPerson person;
    private Action accept;

    public StartingHandPanel Initialize(Transform parent, IBattelPerson person, Action accept)
    {
        (this.person, this.accept) = (person, accept);
        transform.SetParent(parent, false);

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
        accept?.Invoke();
        Destroy(gameObject);
    }
}