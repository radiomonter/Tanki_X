namespace YamlDotNet.Serialization.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class SerializerState : IDisposable
    {
        private readonly IDictionary<Type, object> items = new Dictionary<Type, object>();

        public void Dispose()
        {
            foreach (IDisposable disposable in this.items.Values.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
        }

        public T Get<T>() where T: class, new()
        {
            object obj2;
            if (!this.items.TryGetValue(typeof(T), out obj2))
            {
                obj2 = Activator.CreateInstance<T>();
                this.items.Add(typeof(T), obj2);
            }
            return (T) obj2;
        }

        public void OnDeserialization()
        {
            foreach (IPostDeserializationCallback callback in this.items.Values.OfType<IPostDeserializationCallback>())
            {
                callback.OnDeserialization();
            }
        }
    }
}

