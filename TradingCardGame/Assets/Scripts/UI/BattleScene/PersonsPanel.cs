using UnityEngine;

public class PersonsPanel : MonoBehaviour, IPersonsPanel
{
    [SerializeField] private PersonUI personEnemy = null, personPlayer = null;

    public IPersonsPanel Initialize(Transform parent, IBattelPerson player, IBattelPerson enemy)
    {
        personPlayer.Build(player);
        personEnemy.Build(enemy);

        transform.SetParent(parent, false);

        return this;
    }
}