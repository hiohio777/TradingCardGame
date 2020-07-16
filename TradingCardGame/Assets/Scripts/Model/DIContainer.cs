using System;
using System.Collections.Generic;

/// <summary>
/// DI контейнер
/// </summary>
public class DIContainer : ISinglton
{
    private class DIDataObject<T> : IDataObjectType
    {
        private readonly Func<T> funcCreator;
        private T singlObject;
        private bool isSinglton = false;
        private Func<T> funcRequest; // Меняется в зависимости от того назначен ли тип для синглтона

        public DIDataObject(Func<T> funcCreator)
        {
            this.funcCreator = funcCreator ?? throw new ArgumentNullException(nameof(funcCreator));
            funcRequest = GetNewObject;
        }

        // Обьект который будет создан по запросу, будет синглтоном
        public void SetAsSingleton()
        {
            funcRequest = GetNewSinglton;
            isSinglton = true;
        }

        // Запросить обьект
        public T Request() => funcRequest.Invoke();
        public Func<T> ResolveFunc() => isSinglton == false ? funcCreator
            : throw new InvalidCastException($"DIContainer: Cannot return function! This property is Singleton {typeof(T)}");

        private T GetNewObject() => funcCreator.Invoke();
        private T GetNewSinglton()
        {
            funcRequest = GetSinglton;
            return singlObject = funcCreator.Invoke();
        }
        private T GetSinglton() => singlObject;
    }

    private readonly Dictionary<Type, IDataObjectType> _createFunction
        = new Dictionary<Type, IDataObjectType>();
    private IDataObjectType current;

    void ISinglton.Singl()
    {
        current.SetAsSingleton();
        current = null;
    }

    public ISinglton Bind<T>(Func<T> action)
    {
        Type currentType = typeof(T);
        if (_createFunction.ContainsKey(currentType))
            throw new InvalidCastException($"DIContainer: Type already registered {typeof(T)}");

        _createFunction[currentType] = current = new DIDataObject<T>(action);
        return this;
    }

    public T Get<T>()
    {
        if (_createFunction.TryGetValue(typeof(T), out IDataObjectType value))
            return ((DIDataObject<T>)value).Request();
        throw new InvalidCastException($"DIContainer: invalid resolve object {typeof(T)}");
    }

    public Func<T> GetFunc<T>()
    {
        if (_createFunction.TryGetValue(typeof(T), out IDataObjectType value))
            return ((DIDataObject<T>)value).ResolveFunc();

        throw new InvalidCastException($"DIContainer: invalid resolve object {typeof(T)}");
    }
}

public interface IDataObjectType
{
    void SetAsSingleton();
}

public interface ISinglton
{
    void Singl();
}