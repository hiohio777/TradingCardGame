public class BuffUIParametersFactory : FactoryBase<BuffUIParameters>, IBuffUIParametersFactory
{
    public IBuffUIParameters GetBuffUI()
    {
        BuffUIParameters buffParam;

        if (buffer.Count > 0) buffParam = buffer.Pop();
        else buffParam = BuffUIParameters.CreatPrefab(Buffered);

        return buffParam;
    }
}
