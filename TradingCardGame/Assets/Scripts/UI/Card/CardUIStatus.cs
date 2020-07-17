using UnityEngine;
using UnityEngine.UI;

public class CardUIStatus : MonoBehaviour
{
    public StatusCardEnum StatusCard { get => statusCard; set { SetStatus(value); } }
    public StatusCardEnum statusCard = StatusCardEnum.normal;

    [SerializeField] private LocalisationText status = null;
    [SerializeField] private Image image;

    public void DestroyUI() => Destroy(gameObject);

    private void SetStatus(StatusCardEnum statusCard)
    {
        this.statusCard = statusCard;

        switch (statusCard)
        {
            case StatusCardEnum.normal:
                gameObject.SetActive(false);
                return;
            case StatusCardEnum.not_available:
                image.color = Color.red;
                status.SetKey(statusCard.ToString());
                break;
            case StatusCardEnum.in_the_deck:
                image.color = Color.green;
                status.SetKey(statusCard.ToString());
                break;
            default:
                break;
        }
        gameObject.SetActive(true);
    }
}