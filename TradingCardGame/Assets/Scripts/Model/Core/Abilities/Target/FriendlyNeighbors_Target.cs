using System.Collections.Generic;
using UnityEngine;

public class FriendlyNeighbors_Target : MonoBehaviour, ITargetCards
{
    public List<IAttackCard> GetTargetCards(IAttackCard card, IBattelBase battel)
    {
        var cards = new List<IAttackCard>();
        var cells = card.Warrior.FriendPerson.Cell;

        if (card.Id + 1 < cells.Count)
        {
            if (cells[card.Id + 1].IsExist)
                cards.Add(cells[card.Id + 1].Unit);
        }
        if (card.Id - 1 >= 0)
        {
            if (cells[card.Id - 1].IsExist)
                cards.Add(cells[card.Id - 1].Unit);
        }

        return cards;
    }
}