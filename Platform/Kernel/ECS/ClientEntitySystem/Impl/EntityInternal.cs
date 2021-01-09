namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public interface EntityInternal : Entity
    {
        void AddComponent(Component component);
        void AddComponentSilent(Component component);
        void AddEntityListener(EntityListener entityListener);
        bool CanCast(NodeDescription desc);
        void ChangeComponent(Component component);
        bool Contains(NodeDescription node);
        Node GetNode(NodeClassInstanceDescription instanceDescription);
        void Init();
        void NotifyComponentChange(Type componentType);
        void OnDelete();
        void RemoveComponentSilent(Type componentType);
        void RemoveEntityListener(EntityListener entityListener);
        string ToStringWithComponentsClasses();

        ICollection<Type> ComponentClasses { get; }

        ICollection<Component> Components { get; }

        Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.TemplateAccessor> TemplateAccessor { get; set; }

        bool Alive { get; }

        Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescriptionStorage NodeDescriptionStorage { get; }

        BitSet ComponentsBitId { get; }
    }
}

