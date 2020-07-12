using UnityEngine;

public interface ITokenData
{
    Sprite Icon { get; }
    string Name { get; }
    string Description { get; }
    bool IsSingle { get; }
}

