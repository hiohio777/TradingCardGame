public interface ICardUIStatus
{
    StatusCardEnum StatusCard { get; set; }
    void DestroyUI();
}