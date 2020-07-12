using System;
using System.Collections.Generic;

/// <summary>
/// DI контейнер
/// </summary>
public class DIContainer
{
    private readonly Dictionary<Type, object> _createFunction = new Dictionary<Type, object>();
    private readonly Dictionary<Type, object> _createFunctionSinglton = new Dictionary<Type, object>();
    private readonly Dictionary<Type, object> _objectSinglton = new Dictionary<Type, object>();

    public void Register<T>(Func<T> action)
    {
        Type type = typeof(T);
        if (_createFunction.ContainsKey(type))
            throw new InvalidCastException($"DIContainer: Type already registered {type}");

        _createFunction[type] = action;
    }

    public void RegisterSinglton<T>(Func<T> action)
    {
        Type type = typeof(T);
        if (_createFunctionSinglton.ContainsKey(type))
            throw new InvalidCastException($"DIContainer: Type already registered {type}");

        _createFunctionSinglton[type] = action;
    }

    public T Resolve<T>()
    {
        if (_createFunction.TryGetValue(typeof(T), out object value))
            return ((Func<T>)value).Invoke();

        throw new InvalidCastException($"DIContainer: invalid resolve object {typeof(T)}");
    }

    public Func<T> ResolveFunc<T>()
    {
        if (_createFunction.TryGetValue(typeof(T), out object value))
            return (Func<T>)value;

        throw new InvalidCastException($"DIContainer: invalid resolve object {typeof(T)}");
    }

    public T ResolveSinglton<T>()
    {
        if (_objectSinglton.TryGetValue(typeof(T), out object value))
            return (T)value;

        if (_createFunctionSinglton.TryGetValue(typeof(T), out object singlton))
        {
            var obj = ((Func<T>)singlton).Invoke();
            _objectSinglton.Add(typeof(T), obj);
            return obj;
        }

        throw new InvalidCastException($"DIContainer: invalid resolve object {typeof(T)}");
    }
}