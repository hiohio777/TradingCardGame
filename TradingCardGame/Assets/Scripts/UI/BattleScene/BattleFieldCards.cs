using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldCards : MonoBehaviour, IBattleFieldCards
{
    [SerializeField] private List<AttackCard> AttackingCardsEnemy = null, AttackingCardsPlayer = null;

    public List<IAttackCard> GetAttackingCardsEnemy { get => AttackingCardsEnemy.ConvertAll(x => x as IAttackCard); }
    public List<IAttackCard> GetAttackingCardsPlayer { get => AttackingCardsPlayer.ConvertAll(x => x as IAttackCard); }

    public IBattleFieldCards Initialize()
    {
        for (int i = 0; i < AttackingCardsEnemy.Count; i++)
        {
            AttackingCardsEnemy[i].Id = i;
            AttackingCardsPlayer[i].Id = i;
        }

        return this;
    }

}
