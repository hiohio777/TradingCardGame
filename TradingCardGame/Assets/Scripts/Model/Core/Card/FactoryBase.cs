using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase<T> where T : MonoBehaviour
{
    protected readonly Stack<T> buffer = new Stack<T>();
    protected void Buffered(T obj) => buffer.Push(obj);
    public void ClearBuffer() 
    {
        while (buffer.Count > 0)
        {
            Object.Destroy(buffer.Pop().gameObject);
        }
        buffer.Clear();
    }
}