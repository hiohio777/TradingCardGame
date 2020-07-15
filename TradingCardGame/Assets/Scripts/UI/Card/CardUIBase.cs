using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class CardUIBase : MonoBehaviour, ICardUIBase, IPointerClickHandler
{
    [Space(10)] public CardUIParameters combatUI;
    [Space(10)] public TokensPanel tokensPanel;
    [Space(10)] public Image frame;
    public SpecificityFactory specificityFactory;

    private Transform _transform;
    private Canvas canvasCard;
    private int oldSortingOrder;
    private Action<CardUIBase> buffered;
    private ISpecificity specificity;

    public ICardData CardData { get; private set; }
    public Vector3 Position => _transform.position;
    public Transform CardTransform => _transform;

    public ICardUIBase View { get; }
    public ICardUIBase SetPosition(Vector2 position)
    {
        _transform.position = position;
        return this;
    }

    public ICardUIBase SetSortingOrder(int sortingOrder)
    {
        oldSortingOrder = canvasCard.sortingOrder;
        canvasCard.sortingOrder = sortingOrder;
        return this;
    }

    public ICardUIBase SetOldSortingOrder()
    {
        canvasCard.sortingOrder = oldSortingOrder;
        return this;
    }

    public ICardUIBase SetParent(Transform parent, bool isLast = true)
    {
        _transform.SetParent(parent, false);
        if (isLast) _transform.SetAsLastSibling();
        return this;
    }

    public ICardUIBase SetScale(Vector3 scale)
    {
        transform.localScale = scale;
        return this;
    }

    public ICardUIBase Frame(bool isSelected) => Frame(isSelected, Color.green);
    public ICardUIBase Frame(bool isSelected, Color color)
    {
        frame.color = color;
        frame.gameObject.SetActive(isSelected);
        return this;
    }

    public void StartSpecificity(TypeSpecificityEnum type, Action actFinish)
    {
        if (type == TypeSpecificityEnum.Default) return;
        specificity?.Stop();
        specificity = specificityFactory.GetSpecificity(type, transform, actFinish, () => specificity = null);
    }

    public void Assign(string name, Vector3 scale)
    {
        canvasCard.gameObject.SetActive(true);
        _transform.localScale = scale;
        this.name = name;
    }

    public void Destroy()
    {
        SetSortingOrder(0);
        SetParent(null);
        gameObject.SetActive(false);
        name = "BufferedCard";

        // Поместить\вернуть в буфер для переиспользования
        buffered?.Invoke(this);
    }

    private void Awake()
    {
        canvasCard = GetComponent<Canvas>();
        canvasCard.worldCamera = Camera.main;
        _transform = GetComponent<Transform>();
        frame.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Moving.IsMoving == false)
        {
            clickCard?.Invoke(this);
        }
    }
}

public interface ICardUIBase
{
    Vector3 Position { get; }
    Transform CardTransform { get; }

    ICardUIBase View { get; }
    ICardUIBase SetScale(Vector3 scale);
    ICardUIBase SetParent(Transform parent, bool isLast = true);
    ICardUIBase SetOldSortingOrder();
    ICardUIBase SetSortingOrder(int sortingOrder);
    ICardUIBase SetPosition(Vector2 position);
    ICardUIBase Frame(bool isSelected);
    ICardUIBase Frame(bool isSelected, Color color);

    void StartSpecificity(TypeSpecificityEnum type, Action actFinish);
    void Destroy();
}