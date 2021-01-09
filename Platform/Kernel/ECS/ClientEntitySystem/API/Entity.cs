namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface Entity
    {
        void AddComponent<T>() where T: Component, new();
        void AddComponent(Component component);
        void AddComponent(Type componentType);
        T AddComponentAndGetInstance<T>();
        void AddComponentIfAbsent<T>() where T: Component, new();
        T CreateGroup<T>() where T: GroupComponent;
        Component CreateNewComponentInstance(Type componentType);
        T GetComponent<T>() where T: Component;
        Component GetComponent(Type componentType);
        bool HasComponent<T>() where T: Component;
        bool HasComponent(Type type);
        bool IsSameGroup<T>(Entity otherEntity) where T: GroupComponent;
        void RemoveComponent<T>() where T: Component;
        void RemoveComponent(Type componentType);
        void RemoveComponentIfPresent<T>() where T: Component;
        void ScheduleEvent<T>() where T: Event, new();
        void ScheduleEvent(Event eventInstance);
        T SendEvent<T>(T eventInstance) where T: Event;
        T ToNode<T>() where T: Node, new();

        long Id { get; }

        string Name { get; set; }

        string ConfigPath { get; }

        bool Alive { get; }
    }
}

