using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class AlTrainingBattel : IAlTrainingBattel
{
    public void NextTurn(IBattel battel)
    {
        switch (battel.CurrentBattelState)
        {
            case BattelStateEnum.starting_hand: CreatStarteHand(battel.Enemy); break;
            case BattelStateEnum.reserve: СommitReserve(battel.Enemy); break;
            case BattelStateEnum.tactics: СommitTactics(battel); break;
        }

        battel.ReportReadinessEnemy();
    }

    public void CreatEnemyPerson(IBattelPerson enemy)
    {
        var listCards = new List<string>()
        {
        "CardScriptable 30","CardScriptable 35","CardScriptable 34","CardScriptable 33","CardScriptable 32","CardScriptable 31",
        "CardScriptable 38","CardScriptable 37","CardScriptable 36","CardScriptable 41","CardScriptable 40","CardScriptable 39",
        "CardScriptable 47","CardScriptable 45","CardScriptable 46","CardScriptable 43","CardScriptable 68","CardScriptable 66",
        "CardScriptable 48","CardScriptable 49","CardScriptable 50","CardScriptable 52","CardScriptable 51","CardScriptable 53"
        };
        enemy.Creat("Al_enemy", "horde", listCards, live: 1);
    }

    private void CreatStarteHand(IBattelPerson enemy)
    {
        var cardReserv = new List<string>();
         for (int i = 0; i < 6; i++)
            cardReserv.Add(enemy.DeckCards[i].Name);

        enemy.Report = JsonConvert.SerializeObject(cardReserv);
        enemy.NewStartingHand();
    }

    private void СommitReserve(IBattelPerson enemy)
    {
        //Вывести карты на поле боя
        var cardsAttack = new List<string>();
        var cardReserv = new List<string>();
        enemy.ReservCards.ForEach(x => cardReserv.Add(x.Combat.Name));

        int index = 0;
        foreach (var item in enemy.Cell)
        {
            if (item.IsExist == false)
            {
                cardsAttack.Add(enemy.ReservCards[index].Combat.Name);
                cardReserv.Remove(enemy.ReservCards[index].Combat.Name);
                index++;
            }
            else cardsAttack.Add(item.Unit.Combat.Name);
        }

        var count = cardReserv.Count;
        for (int i = count; i < 6; i++)
            cardReserv.Add(enemy.DeckCards[i - count].Name);

        enemy.Report = new PersonDATAREPORT(cardsAttack, cardReserv).GetJsonString();
        Debug.Log(enemy.Report);
    }

    private void СommitTactics(IBattel battel)
    {
        var random = new System.Random();

        var cards = new List<int>();
        var priceHazard = new List<int>() { -1, -1, -1, -1 };
        foreach (var item in battel.Enemy.AttackCards)
        {
            foreach (var playerCard in battel.Player.AttackCards)
            {
                if (playerCard.Warrior.Enemies.Count >= playerCard.Combat.MaxCountAttackers)
                {
                    priceHazard[playerCard.Id] = -1;
                    continue;
                }

                priceHazard[playerCard.Id] = random.Next(0, 1000);
            }

            int currentPrize = -1, currentIndex = -1;
            for (int i = 0; i < priceHazard.Count; i++)
            {
                if (priceHazard[i] > currentPrize)
                {
                    currentPrize = priceHazard[i];
                    currentIndex = i;
                }
            }

            if (currentIndex >= 0)
                battel.Player.AttackCards[currentIndex].Warrior.Enemies.Add(item.Warrior);
            cards.Add(currentIndex);

            priceHazard.ForEach(x => x = -1);
        }

        battel.Enemy.Report = JsonConvert.SerializeObject(cards);
    }
}
