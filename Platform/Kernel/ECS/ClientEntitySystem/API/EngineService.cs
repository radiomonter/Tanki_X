namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using System;

    public interface EngineService
    {
        void AddComponentListener(ComponentListener componentListener);
        void AddEventListener(EventListener eventListener);
        void AddFlowListener(FlowListener flowListener);
        void AddSystemProcessingListener(EngineHandlerRegistrationListener listener);
        EntityBuilder CreateEntityBuilder();
        void ExecuteInFlow(Consumer<Platform.Kernel.ECS.ClientEntitySystem.API.Engine> consumer);
        void RegisterHandlerFactory(HandlerFactory factory);
        void RegisterSystem(ECSSystem system);
        void RegisterTasksForHandler(Type handlerType);
        void RemoveFlowListener(FlowListener flowListener);

        Platform.Kernel.ECS.ClientEntitySystem.API.Engine Engine { get; }

        TypeInstancesStorage<Event> EventInstancesStorageForReuse { get; }
    }
}

