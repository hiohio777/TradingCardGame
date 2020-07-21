using UnityEngine;
using UnityEngine.UI;

public class PersonUI : MonoBehaviour
{
    private IBattelPerson battelPerson;

    [SerializeField] private Text textNamePeson = null, textCountLive = null;
    [SerializeField] private LocalisationText textFraction = null;
    [SerializeField] private Image fortune = null;

    public void Build(IBattelPerson battelPerson)
    {
        this.battelPerson = battelPerson;
        battelPerson.SetLive += SetCountLive;
        battelPerson.SetFortune += SetFortune;

        textNamePeson.text = battelPerson.Name.ToString();
        textCountLive.text = battelPerson.Live.ToString();
        fortune.gameObject.SetActive(false);

        textFraction.SetKey(battelPerson.Fraction.Name);
    }

    private void SetCountLive(int count)
    {
        textCountLive.text = count.ToString();
    }

    private void SetFortune(bool isActive)
    {
        fortune.gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
        if (battelPerson == null) return;

        battelPerson.SetLive -= SetCountLive;
        battelPerson.SetFortune -= SetFortune;
    }
}
