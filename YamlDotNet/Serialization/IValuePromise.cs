namespace YamlDotNet.Serialization
{
    using System;

    public interface IValuePromise
    {
        event Action<object> ValueAvailable;
    }
}

