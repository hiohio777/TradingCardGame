using System.Collections.Generic;
using UnityEngine;

public class BattleFieldCards : MonoBehaviour
{
    [SerializeField] private List<AttackBattelCell> battelCellEnemy = null, battelCellPlayer = null;

    public List<ICellBattel> CellEnemy => new List<ICellBattel>(battelCellEnemy);
    public List<ICellBattel> CellPlayer => new List<ICellBattel>(battelCellPlayer);
}
