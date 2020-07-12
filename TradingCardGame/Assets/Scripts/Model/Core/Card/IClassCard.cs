using UnityEngine;

public interface IClassCard
{
    Sprite Icon { get; }
    string Name { get; }
    string Description { get; }
    ClassCardEnum Type { get; }
}