using System.Collections.Generic;

public abstract class FactoryBase<T>
{
    protected readonly Stack<T> buffer = new Stack<T>();
    public void ClearBuffer() => buffer.Clear();
    protected void Buffered(T obj) => buffer.Push(obj);
}