using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase<T> where T : MonoBehaviour
{
    protected readonly Stack<T> buffer = new Stack<T>();
    protected readonly List<T> conteiner = new List<T>();
    protected void Buffered(T obj) => buffer.Push(obj);
    public void ClearBuffer()
    {
        conteiner.ForEach(x => Object.Destroy(x.gameObject));
        conteiner.Clear();
        buffer.Clear();
    }
}