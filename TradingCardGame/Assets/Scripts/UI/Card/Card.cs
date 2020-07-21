using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, ICard, IPointerClickHandler
{
    [SerializeField, Space(10)] public CardUIParameters combatUI;
    [SerializeField, Space(10)] public TokensPanel tokensPanel;
    [SerializeField] private MovingCard moving;
    [SerializeField] private CardBase cardBase;
    [SerializeField] private CardUIStatus statusUI;

    private Action<ICard> click;
    private ISFXFactory SFXFactory;
    private ISpecificity SFX;
    private Action<Card> buffered;

    public Card Initial(Action<Card> buffered, ISFXFactory SFXFactory)
    {
        this.SFXFactory = SFXFactory ?? throw new ArgumentNullException(nameof(SFXFactory));
        this.buffered = buffered ?? throw new ArgumentNullException(nameof(buffered));
        return this;
    }

    public ICardData CardData { get; private set; }
    public StatusCardEnum Status { get => statusUI.StatusCard; set => statusUI.StatusCard = value; }
    public IMovingCard Moving => moving;
    public CardBase View => cardBase;

    public void Build(ICardData cardData, Vector3 scale)
    {
        CardData = cardData;
        combatUI.SetInitialValues(CardData);
        View.SetScale(scale);
        this.name = cardData.Name;
        gameObject.SetActive(true);
    }

    public virtual void DestroyUI()
    {
        if (statusUI != null)
            statusUI.StatusCard = StatusCardEnum.normal;
        cardBase.Destroy();
        moving.Destroy();
        gameObject.SetActive(false);
        // Поместить\вернуть в буфер для переиспользования
        buffered?.Invoke(this);
    }

    public void StartSFX(TypeSpecificityEnum type) => StartSFX(type, null);
    public void StartSFX(TypeSpecificityEnum type, Action actFinish)
    {
        if (type == TypeSpecificityEnum.Default) return;
        SFX?.Stop();
        SFX = SFXFactory.GetSpecificity(type, View.CardTransform, actFinish, DestroySpecificity);
    }
    private void DestroySpecificity()
        => SFX = null;

    public void SetClickListener(Action<ICard> click) => this.click = click;
    public virtual void ClearClickListener() => click = null;
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (moving.IsMoving == false)
        {
            click?.Invoke(this);
        }
    }
}
