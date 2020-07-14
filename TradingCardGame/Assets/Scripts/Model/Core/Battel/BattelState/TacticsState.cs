using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public class TacticsState : IBattelState
{
    public BattelStateEnum TypeBattelState { get; } = BattelStateEnum.tactics;
    private IBattelStateData battel;
    private IAttackCard current;

    public void Run(IBattelStateData battel)
    {
        this.battel = battel;

        //Очистить данные
        battel.Player.AttackCards.ForEach(x => { x.AttackTarget = -1; x.Enemies.Clear(); });
        battel.Enemy.AttackCards.ForEach(x => { x.AttackTarget = -1; x.Enemies.Clear(); });

        battel.SetInteractableButtonNextTurn(true);
        battel.Player.AttackCards.ForEach(x => x.SetClickListener(SelectReserveCard));
        battel.Enemy.AttackCards.ForEach(x => x.SetClickListener(AssignAttack));
    }

    public void Request(IBattelStateData battel)
    {
        // внедрить данные о действиях врага в карты на поле боя
        var data = JsonConvert.DeserializeObject<List<int>>(battel.Enemy.Report);
        for (int i = 0; i < data.Count; i++)
        {
            battel.Enemy.AttackCards[i].AttackTarget = data[i];
        }

        Action act = () =>
        {
            battel.BattelSpecific.Rounds++;
            battel.SetBattelState(new RoundState());
        };

        battel.Player.AttackCards.ReturnCardToPlace(act);
    }

    public void ReportReadinessPlayer(Action report)
    {
        current?.Frame(false);
        battel.SetInteractableButtonNextTurn(false);
        battel.Player.AttackCards.ForEach(x => x.ClearClickListener());
        battel.Enemy.AttackCards.ForEach(x => x.ClearClickListener());

        var cards = new List<int>();
        foreach (var item in battel.Player.AttackCards)
        {
            cards.Add(item.AttackTarget);
        }
        battel.Player.Report = JsonConvert.SerializeObject(cards);

        report.Invoke();
    }

    private void SelectReserveCard(IAttackCard attackCard)
    {
        current?.Frame(false);
        if (attackCard != current) (current = attackCard)?.Frame(true);
        else current = null;
    }

    private void AssignAttack(IAttackCard attackCard)
    {
        if (current == null || attackCard.Enemies.Count >= attackCard.Combat.MaxCountAttackers)
            return;

        attackCard.AddAttacker(current);
        current.SetClickListener(CancelAttacker);
        current = null;
    }

    private void CancelAttacker(IAttackCard attackCard)
    {
        attackCard.EnemyPerson.Cell[attackCard.AttackTarget].Unit.RemoveAttacker(attackCard);
        attackCard.SetClickListener(SelectReserveCard);
    }
}