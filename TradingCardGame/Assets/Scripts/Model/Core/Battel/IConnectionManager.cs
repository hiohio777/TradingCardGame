public interface IPhotonConnection
{
    void ToCallRPC(string message);
    void Build(IBattelStateData battel);
}