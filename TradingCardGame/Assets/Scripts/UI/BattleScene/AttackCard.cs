using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : MonoBehaviour, IAttackCard
{
    public int Id { get; set; }
    public Vector3 DefaultPosition { get => _transform.position; }
    public Vector3 CurrentPosotion { get => BattelCard.Posotion; }

    public IBattelCard BattelCard { get; private set; }

    public List<IAttackCard> Enemies { get; set; } = new List<IAttackCard>();
    public IAttackCard AttackCardTarget { get; set; }

    public TypePersonEnum TypePerson { get; private set; }
    public IBattelPerson FriendPerson { get; private set; }
    public IBattelPerson EnemyPerson { get; private set; }
    public bool Fortune { get => FriendPerson.Fortune; }

    private Action<IAttackCard> click;
    private bool isMouse;
    private Transform _transform;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        _transform = transform;
        StartCoroutine(OnMouseButtonUp());
    }

    public void Build(IBattelPerson friend, IBattelPerson enemy, TypePersonEnum typePerson) =>
    (FriendPerson, EnemyPerson, TypePerson) = (friend, enemy, typePerson);

    public void PutCardFromReserve(IBattelCard battelCard)
    {
        if (this.BattelCard != null)
        {
            Debug.Log("В ячейке уже есть назначенная карта!");
            return;
        }

        BattelCard = battelCard;

        BattelCard.Moving.SetPosition(DefaultPosition).SetRotation(0);
        BattelCard.Combat.AttackCard = this;
        StopAllCoroutines();
    }

    public void SetClickListener(Action<IAttackCard> click) => this.click = click;
    public void ClearClickListener() => click = null;

    public void ReturnCardToPlace(Action execute = null, float time = 0.3f)
    {
        if (BattelCard == null) return;

        BattelCard.SetSortingOrder(Id);
        BattelCard.MoveTo(time, DefaultPosition, execute: () => { BattelCard.SetSortingOrder(0); execute.Invoke(); BattelCard.SetSortingOrder(Id); });
    }

    private void OnMouseEnter() => isMouse = true;
    private void OnMouseExit() => isMouse = false;

    private IEnumerator OnMouseButtonUp()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
                if (isMouse) click?.Invoke(this);
            yield return null;
        }
    }

    private readonly int x1 = 30, x2 = 50, y1 = 110, y2 = 150;
    public void AddAttacker(IAttackCard newAttacker)
    {

        if (Enemies.Count > 0)
        {
            if (Enemies[0].BattelCard.Combat.Initiative > newAttacker.BattelCard.Combat.Initiative)
            {
                // Поменять атакующие карты местами
                Action actFinish = () => { newAttacker.BattelCard.Frame(false); newAttacker.BattelCard.SetSortingOrder(1); };
                newAttacker.BattelCard.MoveTo(0.3f, new Vector3(DefaultPosition.x + x1, DefaultPosition.y - y1, 0), execute: actFinish);
                Enemies[0].BattelCard.SetSortingOrder(2);
                Enemies[0].BattelCard.MoveTo(0.3f, new Vector3(DefaultPosition.x + x2, DefaultPosition.y - y2, 0));
            }
            else
            {
                // Назначить второй атакующей
                Action actFinish = () => { newAttacker.BattelCard.Frame(false); newAttacker.BattelCard.SetSortingOrder(2); };
                newAttacker.BattelCard.MoveTo(0.3f, new Vector3(DefaultPosition.x + x2, DefaultPosition.y - y2, 0), execute: actFinish);
            }
        }
        else
        {
            // Назначить первой атакующей
            Action actFinish = () => { newAttacker.BattelCard.Frame(false); newAttacker.BattelCard.SetSortingOrder(1); };
            newAttacker.BattelCard.MoveTo(0.3f, new Vector3(DefaultPosition.x + x1, DefaultPosition.y - y1, 0), execute: actFinish);
        }

        Enemies.Add(newAttacker);
        newAttacker.AttackCardTarget = this;
    }

    public void RemoveAttacker(IAttackCard attacker)
    {
        Enemies.Remove(attacker);

        if (Enemies.Count > 0)
            Enemies[0].BattelCard.SetSortingOrder(1).MoveTo(0.3f, new Vector3(DefaultPosition.x + x1, DefaultPosition.y - y1, 0));

        attacker.AttackCardTarget = null;

        attacker.BattelCard.Frame(true);
        Action actFinish = () => { attacker.BattelCard.Frame(false); attacker.BattelCard.SetSortingOrder(0); };
        attacker.BattelCard.MoveTo(0.3f, attacker.DefaultPosition, execute: actFinish);
    }

    public void ExecuteAbility(EventTriggerEnum trigger, IBattelBase battel, Action finish)
    {
        if (BattelCard != null)
        {
            BattelCard.Frame(true);
            BattelCard.Ability.TriggerEvent(trigger, this, battel, () => { BattelCard.Frame(false); finish?.Invoke(); });
        }
        else finish?.Invoke();
    }

    public void Death(Action act)
    {
        BattelCard.StopSpecificity();
        BattelCard.SetSortingOrder(300);

        Action destroy = () =>
        {
            Pop().Destroy();
            FriendPerson.Live -= 1;
            act.Invoke();
        };

        var deathEndPosition = new Vector3(-1000, 80);
        BattelCard.Moving.SetPosition(deathEndPosition).SetRotation(300).Run(0.5f, destroy);
    }

    public IBattelCard Pop()
    {
        var temp = BattelCard;
        BattelCard.Combat.AttackCard = null;
        BattelCard = null; // Карта убрана из ячейки
        StartCoroutine(OnMouseButtonUp());
        return temp;
    }
}