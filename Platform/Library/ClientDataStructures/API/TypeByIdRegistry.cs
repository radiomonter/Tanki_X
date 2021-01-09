namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public class TypeByIdRegistry
    {
        private readonly IBiMap<Type, long> storage = new HashBiMap<Type, long>();
        private readonly Func<Type, long> idGenerator;

        public TypeByIdRegistry(Func<Type, long> idGenerator)
        {
            this.idGenerator = idGenerator;
        }

        protected internal virtual Type GetClass(long id)
        {
            Type type;
            if (!this.storage.Inverse.TryGetValue(id, out type))
            {
                throw new ClassNotFoundException(id);
            }
            return type;
        }

        protected virtual long GetId(Type clazz)
        {
            long num;
            return (!this.storage.TryGetValue(clazz, out num) ? this.Register(clazz) : num);
        }

        protected virtual long Register(Type clazz)
        {
            long num;
            if (!this.storage.TryGetValue(clazz, out num))
            {
                num = this.idGenerator(clazz);
                if (this.storage.Inverse.ContainsKey(num))
                {
                    throw new TypeAlreadyRegisteredException(clazz);
                }
                this.storage.Add(clazz, num);
            }
            return num;
        }
    }
}

