using System.Collections;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "TokensScriptableObject", menuName = "Data/Token", order = 53)]
public class TokenScriptableObject : ScriptableObject, ITokenData
{
    [SerializeField, Space(10)] private Sprite icon = null;
    [SerializeField] private bool isSingle = false;

    public Sprite Icon => icon;
    public string Name => name;
    public string Description => $"{name}_description";
    public bool IsSingle => isSingle;
}