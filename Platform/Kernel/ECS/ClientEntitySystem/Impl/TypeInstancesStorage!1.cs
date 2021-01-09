namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class TypeInstancesStorage<T>
    {
        private static readonly Dictionary<Type, T> Storage;

        static TypeInstancesStorage()
        {
            TypeInstancesStorage<T>.Storage = new Dictionary<Type, T>();
        }

        public void AddInstance(Type type)
        {
            if (!TypeInstancesStorage<T>.Storage.ContainsKey(type))
            {
                TypeInstancesStorage<T>.Storage.Add(type, (T) Activator.CreateInstance(type));
            }
        }

        public bool HasInstance(Type type) => 
            TypeInstancesStorage<T>.Storage.ContainsKey(type);

        public bool TryGetInstance(Type type, out T instance) => 
            TypeInstancesStorage<T>.Storage.TryGetValue(type, out instance);
    }
}

