using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, ICard, IBattelCard, IPointerClickHandler
{
    public ICardData CardData { get; private set; }
    public ICombatCard Combat { get; private set; }

    public ITokensPanel Tokens => tokensPanel;
    public IAbility Ability => ability;
    public IAbility ability;

    public StatusCardEnum Status { get => baseUI.StatusUI.StatusCard; set => baseUI.StatusUI.StatusCard = value; }

    [Space(10)] public CardUIImages imagesUI = null;
    [Space(10)] public CardUIParameters combatUI = null;
    [Space(10)] public TokensPanel tokensPanel = null;
    private CardUIBase baseUI;

    public Vector3 Scale { get; private set; }
    public bool IsBlocked { get => baseUI.IsBlocked; }

    private Action<ICard> clickCard;
    private Action<IBattelCard> clickBattelCard;
    private Action<IAttackCard> clickAttackCard;

    private ISpecificityFactory specificityFactory;
    private ISpecificity specificity;
    private Action<CardUI> buffered;

    public CardUI Initialize(Action<CardUI> buffered, ISpecificityFactory specificityFactory)
    {
        this.specificityFactory = specificityFactory;
        this.buffered = buffered;
        baseUI = GetComponent<CardUIBase>();
        return this;
    }

    public IBattelCard BuildBattelCard(ICardData cardData, Vector3 scale, ICombatCard combat)
    {
        Build(cardData, scale);
        Combat = combat;
        return this;
    }

    public ICard Build(ICardData cardData, Vector3 scale)
    {
        CardData = cardData;
        baseUI.Build(cardData.Name, scale);

        combatUI.SetInitialValues(CardData);
        imagesUI.SetInitialValues(CardData);
        return this;
    }

    public void StopSpecificity() => specificity?.Stop();
    public void StartSpecificity(TypeSpecificityEnum type) => StartSpecificity(type, null);
    public void StartSpecificity(TypeSpecificityEnum type, Action actFinish)
    {
        if (type == TypeSpecificityEnum.Default) return;
        specificity?.Stop();
        specificity = specificityFactory.GetSpecificity(type, baseUI.TransformCard, actFinish, DestroySpecificity);
    }
          
    public void SetCardUIStatus(ICardUIStatus cardUIStatus) => baseUI.StatusUI = cardUIStatus;

    public void MoveTo(float time, Vector3 target, Action execute = null, float waitTime = 0) =>
        baseUI.SetSortingOrder(100).Moving.SetPosition(target).SetWaitTime(waitTime).Run(time, () => { SetOldSortingOrder(); execute?.Invoke(); });
    public void StopMoveing() => baseUI.Moving.Stop();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsBlocked == false)
        {
            clickCard?.Invoke(this);
            clickBattelCard?.Invoke(this);
            clickAttackCard?.Invoke(Combat.AttackCard);
        }
    }

    public void SetClickListener(Action<ICard> click) => this.clickCard = click;
    public void SetClickListener(Action<IBattelCard> clickBattelCard) => this.clickBattelCard = clickBattelCard;
    public void SetClickListener(Action<IAttackCard> clickAttackCard) => this.clickAttackCard = clickAttackCard;
    public void ClearClickListener()
    {
        clickCard = null;
        clickBattelCard = null;
        clickAttackCard = null;
    }

    public void Frame(bool isSelected) => Frame(isSelected, Color.green);
    public void Frame(bool isSelected, Color color) => baseUI.Frame(isSelected, color);

    public Vector3 Posotion => baseUI.TransformCard.position;
    public IMovingCard Moving => baseUI.Moving;
    public Transform TransformCard => baseUI.TransformCard;
    public Vector2 Size => baseUI.CanvasRectTransform.sizeDelta;
    public ICard SetParent(Transform parent) => baseUI.SetParent(parent);
    public ICard SetPosition(Vector2 position) => baseUI.SetPosition(position);
    public ICard SetSortingOrder(int sortingOrder) => baseUI.SetSortingOrder(sortingOrder);
    public ICard SetOldSortingOrder() => baseUI.SetOldSortingOrder();
    public ICard SetScale(Vector3 scale) => baseUI.SetScale(scale);
    public ICard SetRotation(float rotation) => baseUI.SetRotation(rotation);

    public void Destroy()
    {
        ClearClickListener();
        baseUI.Destroy();
        Ability?.Destroy();
        buffered?.Invoke(this); // Поместить\вернуть в буфер для переиспользования
    }

    private void DestroySpecificity() => specificity = null;

    public Vector3 DefaultPosition { get; set; }
    public ICellBattel Cell { get; set; }
}
