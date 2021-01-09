namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;

    public interface EngineServiceInternal : EngineService
    {
        void ForceRegisterSystem(ECSSystem system);
        Flow GetFlow();
        void RegisterComponentConstructor(ComponentConstructor componentConstructor);
        void RequireInitState();
        void RunECSKernel();

        Platform.Kernel.ECS.ClientEntitySystem.Impl.SystemRegistry SystemRegistry { get; }

        bool IsRunning { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerCollector HandlerCollector { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.EventMaker EventMaker { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.BroadcastEventHandlerCollector BroadcastEventHandlerCollector { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerStateListener HandlerStateListener { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.DelayedEventManager DelayedEventManager { get; }

        Entity EntityStub { get; }

        ICollection<ComponentConstructor> ComponentConstructors { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.EntityRegistry EntityRegistry { get; }

        NodeCollectorImpl NodeCollector { get; }

        Platform.Kernel.ECS.ClientEntitySystem.API.ComponentBitIdRegistry ComponentBitIdRegistry { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeCache NodeCache { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerContextDataStorage HandlerContextDataStorage { get; }

        ICollection<FlowListener> FlowListeners { get; }

        ICollection<ComponentListener> ComponentListeners { get; }

        ICollection<EventListener> EventListeners { get; }
    }
}

