using UnityEngine;

public interface ICurrentDeckUI
{
    void Build(Transform parent);
    void Init(IDeck deck);
    void CheckDeck(string fraction);
}