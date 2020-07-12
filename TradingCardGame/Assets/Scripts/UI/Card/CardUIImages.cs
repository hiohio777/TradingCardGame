using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardUIImages
{
    [SerializeField] private Image fon, classCard;

    public void SetInitialValues(ICardData cardData)
    {
        classCard.sprite = cardData.Class.Icon;
    }
}
