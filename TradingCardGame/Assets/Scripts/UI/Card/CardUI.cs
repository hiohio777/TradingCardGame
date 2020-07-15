using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, ICard, IPointerClickHandler
{
    private Action<ICard> clickCard;
    public ICardUIStatus cardUIStatus;

    public IMovingCard Moving { get; private set; }
    public ICardUIBase View { get; private set; }
    public ICardData CardData { get; private set; }
    public StatusCardEnum Status { get => cardUIStatus.StatusCard; set => cardUIStatus.StatusCard = value; }

    public void StartSpecificity(TypeSpecificityEnum type, Action actFinish = null)
        => View.StartSpecificity(type, actFinish);

    public void MoveTo(float time, Vector3 target, Action execute = null, float waitTime = 0)
    {
        View.SetSortingOrder(100);
        Moving.SetPosition(target).SetWaitTime(waitTime).Run(time, () => { View.SetOldSortingOrder(); execute?.Invoke(); });
    }

    public void SetClickListener(Action<ICard> click) => this.clickCard = click;
    public void ClearClickListener() => clickCard = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Moving.IsMoving == false)
        {
            clickCard?.Invoke(this);
        }
    }

    public void Destroy()
    {
        View.Destroy();
    }
}
