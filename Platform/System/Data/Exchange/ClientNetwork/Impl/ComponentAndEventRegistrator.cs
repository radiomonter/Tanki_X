namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientProtocol.API;
    using System;

    public class ComponentAndEventRegistrator : EngineHandlerRegistrationListener
    {
        private readonly Protocol protocol;

        public ComponentAndEventRegistrator(EngineService engineService, Protocol protocol)
        {
            this.protocol = protocol;
            engineService.AddSystemProcessingListener(this);
        }

        public bool IsShared(Type type) => 
            type.IsDefined(typeof(Shared), true);

        public void OnHandlerAdded(Handler handler)
        {
            HandlerArgumentsDescription handlerArgumentsDescription = handler.HandlerArgumentsDescription;
            foreach (Type type in handlerArgumentsDescription.ComponentClasses)
            {
                this.Register(type);
            }
            foreach (Type type2 in handlerArgumentsDescription.EventClasses)
            {
                this.Register(type2);
            }
        }

        public void Register(Type type)
        {
            if (this.IsShared(type))
            {
                this.protocol.RegisterTypeWithSerialUid(type);
            }
        }
    }
}

