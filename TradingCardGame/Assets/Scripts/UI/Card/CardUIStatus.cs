using UnityEngine;
using UnityEngine.UI;

public class CardUIStatus : MonoBehaviour
{
    public StatusCardEnum StatusCard { get => statusCard; set { SetStatus(value); } }
    public StatusCardEnum statusCard = StatusCardEnum.normal;

    [SerializeField] private LocalisationText status = null;
    private Image image;

    public static CardUIStatus CreatPrefab(Transform parent) =>
       Instantiate(Resources.Load<CardUIStatus>($"Card/CardUIStatus")).Build(parent);

    public void DestroyUI() => Destroy(gameObject);

    private CardUIStatus Build(Transform parent)
    {
        transform.SetParent(parent, false);
        image = GetComponent<Image>();
        gameObject.SetActive(false);
        return this;
    }

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