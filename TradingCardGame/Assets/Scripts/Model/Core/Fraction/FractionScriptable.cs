using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "FractionScriptable", menuName = "Data/Fraction", order = 52)]
public class FractionScriptable : ScriptableObject, IFraction
{
    [SerializeField, Space(10)] private Sprite icon = null;

    public Sprite Icon => icon;
    public string Name => name;
    public string Description => $"{name}_description";
}
