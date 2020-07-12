using System;
using UnityEngine;

[Serializable]
public class Combat
{
    [Range(1, 8)] public int initiative = 1;
    [Range(1, 60)] public int attack = 1;
    [Range(0, 60)] public int defense = 0;
    [Range(1, 60)] public int health = 1;
    [Range(1, 2)] public int maxCountAttackers = 2;
}