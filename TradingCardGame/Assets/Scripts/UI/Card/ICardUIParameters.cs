public interface ICardUIParameters
{
    void ShowInitiative(int count, int change = 0);
    void ShowAttack(int count, int change = 0);
    void ShowDefense(int count, int change = 0);
    void ShowHealth(int count, int change = 0);
}