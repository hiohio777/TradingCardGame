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
        battel.Player.AttackCards.ForEach(x => { x.AttackCardTarget = null; x.Enemies.Clear(); });
        battel.Enemy.AttackCards.ForEach(x => { x.AttackCardTarget = null; x.Enemies.Clear(); });

        battel.SetInteractableButtonNextTurn(true);
        battel.Player.AttackCards.ForEach(x => x.BattelCard.SetClickListener(SelectReserveCard));
        battel.Enemy.AttackCards.ForEach(x => x.BattelCard.SetClickListener(AssignAttack));
    }

    public void Request(IBattelStateData battel)
    {
        // внедрить данные о действиях врага в карты на поле боя
        var data = JsonConvert.DeserializeObject<List<int>>(battel.Enemy.Report);
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i] == -1) continue;
            battel.Enemy.AttackCards[i].AttackCardTarget = battel.Player.AttackCards[data[i]];
            battel.Player.AttackCards[data[i]].Enemies.Add(battel.Enemy.AttackCards[i]);
        }

        Action act = () =>
        {
            battel.BattelSpecific.Rounds++;
            battel.SetBattelState(new RoundState());
        };

        battel.Player.ReturnCardToPlace(act);
    }

    public void ReportReadinessPlayer(Action report)
    {
        current?.BattelCard.Frame(false);
        battel.SetInteractableButtonNextTurn(false);
        battel.Player.AttackCards.ForEach(x => x.BattelCard.ClearClickListener());
        battel.Enemy.AttackCards.ForEach(x => x.BattelCard.ClearClickListener());

        var cards = new List<int>();
        foreach (var item in battel.Player.AttackCards)
        {
            if (item.AttackCardTarget != null) cards.Add(item.AttackCardTarget.Id);
            else cards.Add(-1);
        }
        battel.Player.Report = JsonConvert.SerializeObject(cards);

        report.Invoke();
    }

    private void SelectReserveCard(IAttackCard attackCard)
    {
        current?.BattelCard.Frame(false);
        if (attackCard != current) (current = attackCard)?.BattelCard.Frame(true);
        else current = null;
    }

    private void AssignAttack(IAttackCard attackCard)
    {
        if (current == null || attackCard.Enemies.Count >= attackCard.BattelCard.Combat.MaxCountAttackers)
            return;

        attackCard.AddAttacker(current);
        current.BattelCard.SetClickListener(CancelAttacker);
        current = null;
    }

    private void CancelAttacker(IAttackCard attackCard)
    {
        attackCard.AttackCardTarget.RemoveAttacker(attackCard);
        attackCard.BattelCard.SetClickListener(SelectReserveCard);
    }
}