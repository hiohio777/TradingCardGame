using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardUIBase : MonoBehaviour
{
    public Transform TransformCard { get; private set; }
    public RectTransform CanvasRectTransform { get; private set; }
    public Canvas CanvasCard { get; private set; }
    public int OldSortingOrder { get; set; }
    public IMovingCard Moving { get; private set; }
    public ICardUIStatus StatusUI { get; set; }
    public bool IsBlocked { get => Moving.IsMoving; }

    [Space(10)] public Image frame = null;
    private ICard card;

    public ICard SetPosition(Vector2 position)
    {
        TransformCard.position = position;
        return card;
    }

    public ICard SetSortingOrder(int sortingOrder)
    {
        OldSortingOrder = CanvasCard.sortingOrder;
        CanvasCard.sortingOrder = sortingOrder;
        return card;
    }

    public ICard SetOldSortingOrder()
    {
        CanvasCard.sortingOrder = OldSortingOrder;
        return card;
    }

    public ICard SetParent(Transform parent)
    {
        TransformCard.SetParent(parent, false);
        TransformCard.SetAsLastSibling();
        return card;
    }

    public ICard SetScale(Vector3 scale)
    {
        transform.localScale = scale;
        return card;
    }

    public ICard SetRotation(float rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        Moving.SetRotations(rotation);
        return card;
    }

    public void Frame(bool isSelected, Color color)
    {
        frame.color = color;
        frame.gameObject.SetActive(isSelected);
    }

    public void Build(string name, Vector3 scale)
    {
        CanvasCard.gameObject.SetActive(true);
        frame.gameObject.SetActive(false);

        TransformCard.localScale = scale;
        this.name = name;
    }

    public void Destroy()
    {
        SetSortingOrder(0).SetScale(new Vector3(1, 1, 1)).SetParent(null).SetRotation(0);
        Moving.ResetData();
        CanvasCard.gameObject.SetActive(false);
        frame.gameObject.SetActive(false);
        name = "BufferedCard";
    }

    private void Awake()
    {
        CanvasCard = GetComponent<Canvas>();
        CanvasCard.worldCamera = Camera.main;
        CanvasRectTransform = GetComponent<RectTransform>();
        TransformCard = GetComponent<Transform>();
        Moving = GetComponent<IMovingCard>();
        card = GetComponent<ICard>();
    }
}

