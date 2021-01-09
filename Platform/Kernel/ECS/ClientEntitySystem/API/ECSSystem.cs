namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ECSSystem : EngineImpl
    {
        protected Entity GetEntityById(long entityId) => 
            Flow.Current.EntityRegistry.GetEntity(entityId);

        public void Init(TemplateRegistry templateRegistry, DelayedEventManager delayedEventManager, EngineServiceInternal engineService, NodeRegistrator nodeRegistrator)
        {
            base.Init(templateRegistry, delayedEventManager);
            this.Log = LoggerProvider.GetLogger(this);
        }

        protected ILog Log { get; private set; }
    }
}

