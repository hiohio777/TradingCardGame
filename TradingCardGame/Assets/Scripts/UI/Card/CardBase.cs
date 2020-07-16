﻿using UnityEngine;
using UnityEngine.UI;

public class CardBase : MonoBehaviour
{
    private Transform _transform;
    private Canvas canvasCard;
    private int oldSortingOrder;
    [SerializeField, Space(10)] private Image frame;

    public Vector3 Position => _transform.position;
    public Transform CardTransform => _transform;

    public CardBase SetPosition(Vector2 position)
    {
        _transform.position = position;
        return this;
    }
    public CardBase SetParent(Transform parent, bool isLast = true)
    {
        _transform.SetParent(parent, false);
        if (isLast) _transform.SetAsLastSibling();
        return this;
    }
    public CardBase SetScale(Vector3 scale)
    {
        transform.localScale = scale;
        return this;
    }
    public CardBase SetSortingOrder(int sortingOrder)
    {
        oldSortingOrder = canvasCard.sortingOrder;
        canvasCard.sortingOrder = sortingOrder;
        return this;
    }
    public CardBase SetOldSortingOrder()
    {
        canvasCard.sortingOrder = oldSortingOrder;
        return this;
    }
    public CardBase Frame(bool isSelected) => Frame(isSelected, Color.green);
    public CardBase Frame(bool isSelected, Color color)
    {
        SetSortingOrder(isSelected ? +200 : -200);
        frame.color = color;
        frame.gameObject.SetActive(isSelected);
        return this;
    }
    private void OnDisable()
    {
        frame.gameObject.SetActive(false);
        SetParent(null);
        SetSortingOrder(0);
        name = "BufferedCard";
    }
    private void Awake()
    {
        canvasCard = GetComponent<Canvas>();
        canvasCard.worldCamera = Camera.main;
        _transform = transform;
        frame.gameObject.SetActive(false);
    }
}
