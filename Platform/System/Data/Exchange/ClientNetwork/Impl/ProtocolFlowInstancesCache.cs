namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class ProtocolFlowInstancesCache : AbstratFlowInstancesCache
    {
        private Dictionary<Type, AbstractCache> instancesCache = new Dictionary<Type, AbstractCache>();

        public ProtocolFlowInstancesCache()
        {
            this.RegisterType<EntityShareCommand>();
            this.RegisterType<EntityUnshareCommand>();
            this.RegisterType<CloseCommand>();
            this.RegisterType<InitTimeCommand>();
            this.RegisterType<SendEventCommand>();
            this.RegisterType<ComponentAddCommand>();
            this.RegisterType<ComponentRemoveCommand>();
            this.RegisterType<ComponentChangeCommand>();
        }

        public T GetInstance<T>() => 
            ((Cache<T>) this.instancesCache[typeof(T)]).GetInstance();

        public object GetInstance(Type type) => 
            !ReferenceEquals(type, typeof(SendEventCommand)) ? (!ReferenceEquals(type, typeof(ComponentAddCommand)) ? (!ReferenceEquals(type, typeof(ComponentRemoveCommand)) ? (!ReferenceEquals(type, typeof(ComponentChangeCommand)) ? (!ReferenceEquals(type, typeof(EntityShareCommand)) ? (!ReferenceEquals(type, typeof(EntityUnshareCommand)) ? (!ReferenceEquals(type, typeof(CloseCommand)) ? (!ReferenceEquals(type, typeof(InitTimeCommand)) ? null : this.GetInstance<InitTimeCommand>()) : this.GetInstance<CloseCommand>()) : this.GetInstance<EntityUnshareCommand>()) : this.GetInstance<EntityShareCommand>()) : this.GetInstance<ComponentChangeCommand>()) : this.GetInstance<ComponentRemoveCommand>()) : this.GetInstance<ComponentAddCommand>()) : this.GetInstance<SendEventCommand>();

        private void RegisterType<T>()
        {
            this.instancesCache.Add(typeof(T), base.Register<T>());
        }
    }
}

